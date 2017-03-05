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
	public int sqNodes;

	Node[,] nodes;

	void Start () {
		nodes = new Node[sqNodes, sqNodes];
		for (int i = 0; i < sqNodes; i++) {
			for (int j = 0; j < sqNodes; j++) {
				//.gotta change this to look at the game field boundaries
				float x = Mathf.Lerp (-40, 40, i / sqNodes);//.or "i / (sqNodes - 1)"?
				float z = Mathf.Lerp (-40, 40, j / sqNodes);//.or "j / (sqNodes - 1)"?
				nodes [i, j] = new Node (new Vector3(x, 0, z));
				Debug.Log (nodes [i, j]);
			}
		}
	}
	
	void Update () {
		//.debug draw etc
	}
}
