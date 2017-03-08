using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]

public class Vehicle : MonoBehaviour {
	public float maxSpeed = 20;
	public float maxForce = 10;

	Vector3 steering;
	Rigidbody rb;

	Vector3 wanderDirection;

	void Awake () {
		steering = Vector3.zero;
		rb = GetComponent<Rigidbody> ();
		wanderDirection = Random.insideUnitCircle.normalized;
	}


	public Vector3 GetSteering () {
		return steering;
	}

	public void ApplyForce (Vector3 force) {
		steering = Vector3.ClampMagnitude (force, maxForce);
		rb.AddForce (steering);
		//		steering = Vector3.zero;

		if (rb.velocity.magnitude > maxSpeed)
			rb.velocity *= 0.99f;
	}


	/** steering force = desired velocity - current velocity */
	public Vector3 Steer (Vector3 desired) {
		return desired - rb.velocity;
	}

	/** Move as fast as possible toward a static point. */
	public Vector3 Seek (Vector3 target) {
		Vector3 desired = (target - transform.position).normalized * maxSpeed;
		return Steer (desired);
	}
	public Vector3 Seek (GameObject obj) { return Seek (obj.transform.position); }

	/** Move as fast as possible away from a static point. */
	public Vector3 Flee (Vector3 target) {
		Vector3 desired = (transform.position - target).normalized * maxSpeed;
		return Steer (desired);
	}
	public Vector3 Flee (GameObject obj) { return Flee (obj.transform.position); }

	/** Seek a target until it's close; then approach (seek) slowly. */
	public Vector3 Arrive (Vector3 target) {
		float radius = maxSpeed;//.find a balance

		Vector3 desired = target - transform.position;
		float d = desired.magnitude;

		desired = desired.normalized * maxSpeed;
		if (d < radius)
			desired *= d / radius;

		return Steer (desired);
	}
	public Vector3 Arrive (GameObject obj) { return Arrive (obj.transform.position); }

	/** Seek an object's future position. */
	public Vector3 Pursue (GameObject obj) {
		Vector3 future = obj.transform.position + obj.GetComponent<Rigidbody> ().velocity;
		return Seek (future);
	}

	/** Flee from an object's future position. */
	public Vector3 Evade (GameObject obj) {
		Vector3 future = obj.transform.position + obj.GetComponent<Rigidbody> ().velocity;
		return Flee (future);
	}

	public Vector3 Wander() {
		float jitter = 10;//.arbitrary
		float angle = Random.value < 0.5f ? jitter : -jitter;

		wanderDirection = Quaternion.AngleAxis (angle, Vector3.up) * wanderDirection;
		wanderDirection = new Vector3 (wanderDirection.x, 0, wanderDirection.z);//.idk why this was necessary

		float length = maxSpeed * 0.4f;//.arbitrary
		float radius = length * 1.2f;//.arbitrary
		Vector3 center = rb.velocity.normalized * length;//.arbitrary
		Vector3 displacement = wanderDirection * radius;
		//		Debug.DrawRay (transform.position, center);
		//		Debug.DrawRay (transform.position + center, displacement);

		Vector3 desired = transform.position + center + displacement;
		return Seek (desired);
	}

	//.Offset pursuit?
	//.Explore
	//.Forage
	//.FollowPath
	//.ContainWithin
	//.Shadow

	/** Steer away from objects. The closer an object, the greater the separation force from that object. */
	public Vector3 Separate (GameObject[] objects) {
		Vector3 sum = Vector3.zero;
		if (objects.Length == 0)
			return sum;

		foreach (GameObject obj in objects) {
			Vector3 away = transform.position - obj.transform.position;
			sum += away.normalized / away.magnitude;
		}

		Vector3 desired = sum.normalized * maxSpeed / objects.Length;

		return Steer (desired * maxSpeed);
	}

	/** Steer in the average direction of other objects. */
	public Vector3 Align (GameObject[] objects) {
		Vector3 dir = Vector3.zero;
		if (objects.Length == 0)
			return dir;

		foreach (GameObject obj in objects)
			dir += obj.GetComponent<Rigidbody> ().velocity;

		return Steer (dir.normalized * maxSpeed);
	}

	/** Arrive at the center of mass of other objects. */
	public Vector3 Cohere (GameObject[] objects) {
		Vector3 center = Vector3.zero;
		if (objects.Length == 0)
			return center;

		foreach (GameObject obj in objects)
			center += obj.transform.position;

		return Arrive (center / objects.Length);
	}

	/** Arrive at a point behind the leader. */
	//.need param for things to separate from? or just handle that in the controller?
	public Vector3 Follow (GameObject leader, float followDistance, float bufferLength) {
		Vector3 lv = leader.GetComponent<Rigidbody> ().velocity;

		float d = (leader.transform.position - gameObject.transform.position).magnitude;

		float bufferRadius = followDistance / 2;

		if (d < bufferRadius) {
			// If way too close, just get away.
			return Flee (leader);
		} else if (d < bufferLength + bufferRadius) {
			// If kinda close, see if they're in front of the leader and evade if necessary.
			RaycastHit hit;
			//.gotta use spherecastall. otherwise it just returns the "first hit"
			if (Physics.SphereCast (leader.transform.position, bufferRadius, lv, out hit, bufferLength))
			if (hit.collider.gameObject == this.gameObject)
				return Evade (leader);
		} else {
			// Arrive at point behind the leader.
			Vector3 desired = leader.transform.position - lv.normalized * followDistance;
			return Arrive (desired);
		}

		return Vector3.zero;
	}

	public Vector3 AvoidObstacles () {
		float seconds = 1;//.find a good balance. make it based on max force?
		float distance = rb.velocity.magnitude * seconds;
		float radius = 2.6f;//.gotta figure out a better way to calculate this

		RaycastHit hit;
		if (Physics.SphereCast (transform.position, radius, rb.velocity, out hit, distance)) {
			if (hit.collider.gameObject.CompareTag ("Obstacle")) {
				Vector3 reflection = Vector3.Reflect (hit.point - transform.position, hit.normal) * 1.1f;
				//Debug.DrawLine (transform.position, hit.point, Color.black);
				//Debug.DrawLine (hit.point, transform.position + reflection, Color.black);
				Vector3 away = transform.position + reflection;
				return Seek (away);

			}
		}

		return Vector3.zero;
	}

	/*
	public Vector3 AvoidCollisions () {
		//.find a good balance
		float seconds = 1;
		float distance = rb.velocity.magnitude * seconds;

		foreach (Collider other in Physics.OverlapSphere (transform.position, distance)) {
			Rigidbody rb2 = other.gameObject.GetComponent<Rigidbody> ();

			if (rb2 != null) {
				Vector3 p1;
				Vector3 p2;
				// if the lines aren't parallel
				if (Math3d.ClosestPointsOnTwoLines (out p1, out p2, transform.position, rb.velocity, other.gameObject.transform.position, rb2.velocity)) {
					float t1 = (p1 - transform.position).sqrMagnitude / rb.velocity.magnitude;
					float t2 = (p2 - other.gameObject.transform.position).sqrMagnitude / rb2.velocity.magnitude;
					// if the "collision" is at the same space-time
					if (Mathf.Abs (t2 - t1) < 1) {
						// if the collision is close enough to detect
						if ((p1 - transform.position).magnitude < distance) {
							if ((p2 - p1).magnitude < 1) {//.use object radii instead
								return Evade (other.gameObject);
							}
						}
					}
				}
			}
		}

		return Vector3.zero;
	}
	*/

	/*
	public Vector3 Queue () {
		float distance = rb.velocity.magnitude;//.

		RaycastHit hit;
		if (Physics.Raycast (transform.position, rb.velocity, distance)) {
			return -rb.velocity * 0.8f;//.arbitrary
		}

		return Vector3.zero;
	}
	*/
}