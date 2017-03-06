using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {
	public float speed;
	public Transform player;
	public Pathfinder pathfinder;

	Rigidbody rb;
	List<Vector3> path;
	Vector3 target;

	void Start () {
		rb = GetComponent<Rigidbody> ();
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
		}

		path = pathfinder.GetPath (transform.position, target);

		//rb.AddForce (Vector3.MoveTowards (transform.position, path [2], Time.deltaTime * speed));
		transform.position = Vector3.MoveTowards (transform.position, path [2], speed * Time.deltaTime);
	}

	void OnDrawGizmos () {for (int i = 0; i < path.Count; i++) {
			Gizmos.color = Color.Lerp (Color.blue, Color.green, (float)i / path.Count);
			Gizmos.DrawSphere (path [i], 0.5f);
		}
	}
}
