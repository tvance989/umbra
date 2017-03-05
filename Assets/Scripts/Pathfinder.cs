using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Node {
	public Node parent;
	public Vector3 position;
	public bool isWalkable;
	public float f, g, h;
	public NodeState state;

	public Node(Vector3 pos) {
		position = pos;
	}
}
enum NodeState { Untested, Open, Closed }

public class Pathfinder : MonoBehaviour {
	public Grid grid;

	Node[,] nodes;

	void Start () {
		nodes = new Node[grid.vertices.GetLength (0), grid.vertices.GetLength (1)];

		for (int i = 0; i < grid.vertices.GetLength (0); i++) {
			for (int j = 0; j < grid.vertices.GetLength (1); j++) {
				nodes [i, j] = new Node (grid.vertices [i, j]);
			}
		}
	}

	void OnDrawGizmos () {
		if (nodes == null)
			return;

		Gizmos.color = Color.black;
		foreach (Node node in nodes) {
			Gizmos.DrawSphere (node.position, 0.3f);
		}

		Gizmos.color = Color.green;
		foreach (Node node in GetNeighborNodes(0, 4)) {
			Gizmos.DrawSphere (node.position, 0.3f);
		}
	}

	List<Node> GetNeighborNodes (int i, int j) {
		Debug.Log ("neighbors for " + i + " " + j);
		List<Node> neighbors = new List<Node> ();
		int iMax = nodes.GetLength (0);
		int jMax = nodes.GetLength (1);

		for (int dx = (i > 0 ? -1 : 0); dx <= (i < iMax ? 1 : 0); dx++) {
			for (int dz = (j > 0 ? -1 : 0); dz <= (j < jMax ? 1 : 0); dz++) {
				if (dx != 0 || dz != 0) {
					neighbors.Add (nodes [i + dx, j + dz]);
					Debug.Log ("neighbor " + (i + dx) + " " + (j + dz));
				}
			}
		}

		return neighbors;
	}
}
