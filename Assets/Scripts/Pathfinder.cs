using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Node {
	public Node parent;
	public Vector3 position;
	public int i, j;
	//public bool? isWalkable;
	public float f, g, h;
	public NodeState state;

	public Node(int I, int J, Vector3 pos) {
		i = I;
		j = J;
		position = pos;
	}
}
enum NodeState { Untested, Open, Closed }

public class Pathfinder : MonoBehaviour {
	public Grid grid;
	//public GameObject sun, player;//.temp

	Node[,] nodes;

	void Start () {
		nodes = new Node[grid.vertices.GetLength (0), grid.vertices.GetLength (1)];

		for (int i = 0; i < grid.vertices.GetLength (0); i++) {
			for (int j = 0; j < grid.vertices.GetLength (1); j++) {
				nodes [i, j] = new Node (i, j, grid.vertices [i, j]);
			}
		}
	}

	public List<Vector3> GetPath (Vector3 a, Vector3 b) {
		List<Vector3> path = new List<Vector3> ();

		Node current = AStar (GetClosestWalkableNode (a), GetClosestWalkableNode (b));
		path.Add (current.position);

		while (current.parent != null) {
			current = current.parent;
			path.Add (current.position);
		}

		path.Reverse ();
		return path;
	}

	List<Node> GetNeighborNodes (Node node) {
		//Debug.Log ("neighbors for " + node.i + " " + node.j);
		List<Node> neighbors = new List<Node> ();
		int iMax = nodes.GetLength (0);
		int jMax = nodes.GetLength (1);

		for (int di = (node.i > 0 ? -1 : 0); di <= (node.i < iMax ? 1 : 0); di++) {
			for (int dj = (node.j > 0 ? -1 : 0); dj <= (node.j < jMax ? 1 : 0); dj++) {
				if (di != 0 || dj != 0) {
					neighbors.Add (nodes [node.i + di, node.j + dj]);
					//Debug.Log ("neighbor at " + (node.i + di) + " " + (node.j + dj));
				}
			}
		}

		return neighbors;
	}

	List<Node> GetWalkableNodes (Node node) {
		//Debug.Log ("walkables for " + node.i + " " + node.j);
		List<Node> walkables = new List<Node> ();

		foreach (Node neighbor in GetNeighborNodes (node)) {
			if (NodeIsWalkable (neighbor)) {
				//Debug.Log ("walkable at " + (neighbor.i) + " " + (neighbor.j));
				walkables.Add (neighbor);
			}
		}

		return walkables;
	}

	bool NodeIsWalkable (Node node) {
		/*if (node.isWalkable.HasValue)
			return node.isWalkable;*/

		bool walkable = true;
		foreach (Collider coll in Physics.OverlapSphere(node.position, 4f)) {//.arbitrary radius. need sun's radius with wiggle room.
			if (coll.gameObject.CompareTag ("Obstacle")) {
				walkable = false;
			}
		}

		return walkable;
	}

	// O(n * m)
	//."walkable code" is beta. remove it?
	Node GetClosestWalkableNode (Vector3 pos) {
		int iMin = 0;
		int jMin = 0;
		int iMax = nodes.GetLength (0) - 1;
		int jMax = nodes.GetLength (1) - 1;

		Node closestNode = nodes [iMin, jMin];
		float closestDist = Mathf.Infinity;
		float thisD;
		for (int i = iMin; i <= iMax; i++) {
			for (int j = jMin; j <= jMax; j++) {
				thisD = (nodes [i, j].position - pos).sqrMagnitude;
				if (thisD < closestDist && NodeIsWalkable (nodes [i, j])) {
					closestNode = nodes [i, j];
					closestDist = thisD;
				}
			}
		}

		return closestNode;
	}

	Node AStar (Node start, Node goal) {//.dont return node. return path.
		List<Node> closed = new List<Node> ();
		List<Node> open = new List<Node> ();

		foreach (Node node in nodes) {
			node.parent = null;
			node.g = node.f = Mathf.Infinity;
			node.state = NodeState.Untested;
		}

		open.Add (start);
		start.state = NodeState.Open;
		start.g = 0;
		start.h = HCost (start, goal);
		start.f = start.g + start.h;

		while (open.Count > 0) {
			Node current = LowestF (open);

			if (current == goal) {
				//.reconstruct path
				return current;//.return path instead
			}

			open.Remove (current);
			closed.Add (current);

			foreach (Node neighbor in GetWalkableNodes(current)) {
				if (neighbor.state == NodeState.Closed)
					continue;
				
				float g = current.g + (neighbor.position - current.position).magnitude;
				if (neighbor.state != NodeState.Open) {
					open.Add (neighbor);
					neighbor.state = NodeState.Open;
				} else if (g >= neighbor.g) {
					continue;
				}

				neighbor.parent = current;
				neighbor.g = g;
				neighbor.h = HCost (neighbor, goal);
				neighbor.f = neighbor.g + neighbor.h;
			}
		}

		return nodes [0, 0];//.bad
	}

	float HCost (Node node, Node goal) {
		return (goal.position - node.position).magnitude;
	}

	Node LowestF (List<Node> ns) {
		Node low = ns [0];
		foreach (var n in ns) {
			if (n.f < low.f) {
				low = n;
			}
		}
		return low;
	}
}
