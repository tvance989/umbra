using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {
	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody> ();

		/*for (int i = 0; i < grid.vertices.GetLength (0); i++) {
			for (int j = 0; j < grid.vertices.GetLength (1); j++) {
				Debug.Log ("vert " + i + " " + j);
				Debug.Log (grid.vertices [i, j]);
			}
		}*/
	}
	
	void Update () {
		
	}
}
