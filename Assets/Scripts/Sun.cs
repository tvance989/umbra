using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {
	public float speed;
	public Transform player;
	public Pathfinder pathfinder;

	Vehicle vehicle;
	List<Vector3> path;
	Vector3 target;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
		path = new List<Vector3> ();
		target = player.position;
		target.y = transform.position.y;
	}
	
	void FixedUpdate () {
		Vector3 force = Vector3.zero;

		if (CanSeePlayer ()) {
			target = player.position;
			target.y = transform.position.y;
			force += vehicle.Arrive (target);
			path.Clear ();
		} else {
			path = pathfinder.GetPath (transform.position, target);

			if (path.Count > 1) {
				Vector3 seek = path [1];
				seek.y = transform.position.y;
				force += vehicle.Seek (seek);
			} else {
				// If the sun reaches the end of its path and still doesn't see the player,
				// the sun will have a flash of inspiration and know where the player is.
				target = player.position;
			}
		}

		force += vehicle.AvoidObstacles () * 2;

		Debug.DrawLine (transform.position, transform.position + force);
		vehicle.ApplyForce (force);
	}

	bool CanSeePlayer () {
		RaycastHit hit;
		Physics.Raycast (transform.position, player.position - transform.position, out hit);
		return hit.collider.gameObject == player.gameObject;
	}

	void OnDrawGizmos () {
		if (path == null)
			return;
		for (int i = 0; i < path.Count; i++) {
			Gizmos.color = Color.Lerp (Color.blue, Color.green, (float)i / path.Count);
			Gizmos.DrawSphere (path [i], 0.5f);
		}
		Gizmos.color = Color.red;
		if (CanSeePlayer ())
			Gizmos.DrawLine (transform.position, player.position);
	}
}
