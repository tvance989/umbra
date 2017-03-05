using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {
	public int numX, numZ, sizeX, sizeZ;

	Vector3[] vertices;

	void Awake () {
		Generate ();
	}

	void Generate () {
		vertices = new Vector3[(numX + 1) * (numZ + 1)];

		float xMin = transform.position.x - sizeX / 2;
		float xMax = transform.position.x + sizeX / 2;
		float zMin = transform.position.z - sizeZ / 2;
		float zMax = transform.position.z + sizeZ / 2;

		for (int i = 0, z = 0; z <= numZ; z++) {
			for (int x = 0; x <= numX; x++, i++) {
				float px = Mathf.Lerp (xMin, xMax, (float)x / numX);
				float pz = Mathf.Lerp (zMin, zMax, (float)z / numZ);
				vertices [i] = new Vector3 (px, 0, pz);
				Debug.Log (vertices [i]);
			}
		}
	}

	void OnDrawGizmos () {
		if (vertices == null)
			return;

		Gizmos.color = Color.black;
		for (int i = 0; i < vertices.Length; i++) {
			Gizmos.DrawSphere (vertices [i], 0.5f);
		}
	}
}
