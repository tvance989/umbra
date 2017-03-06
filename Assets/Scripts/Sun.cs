using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {
	public float speed;
	public Transform player;
	public Pathfinder pathfinder;

	Rigidbody rb;
	Vehicle vehicle;
	List<Vector3> path;
	Vector3 target;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		vehicle = GetComponent<Vehicle> ();
		path = new List<Vector3> ();
		target = player.position;

		/*for (int i = 0; i < grid.vertices.GetLength (0); i++) {
			for (int j = 0; j < grid.vertices.GetLength (1); j++) {
				Debug.Log ("vert " + i + " " + j);
				Debug.Log (grid.vertices [i, j]);
			}
		}*/
	}
	
	void FixedUpdate () {
		RaycastHit hit;
		Physics.Raycast (transform.position, player.position - transform.position, out hit);
		if (hit.collider.gameObject == player.gameObject) {
			target = player.position;
			vehicle.ApplyForce (vehicle.Arrive (target));
			return;
		}
		//target = player.position;

		path = pathfinder.GetPath (transform.position, target);

		Vector3 force = Vector3.zero;

		if (path.Count > 1) {
			Vector3 seek = path [1];
			seek.y = transform.position.y;
			force += vehicle.Seek (seek);
		} else {
			target = player.position;
		}

		vehicle.ApplyForce (force);
		//Debug.DrawLine (transform.position, transform.position + force);

		//rb.AddForce (Vector3.MoveTowards (transform.position, path [2], Time.deltaTime * speed));
		//transform.position = Vector3.MoveTowards (transform.position, path [2], speed * Time.deltaTime);
	}

	void OnDrawGizmos () {
		if (path == null)
			return;
		for (int i = 0; i < path.Count; i++) {
			Gizmos.color = Color.Lerp (Color.blue, Color.green, (float)i / path.Count);
			Gizmos.DrawSphere (path [i], 0.5f);
		}
	}
}
