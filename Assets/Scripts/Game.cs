using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}

public class Game : MonoBehaviour {
	public Boundary boundary;
	public GameObject obstacle, pickup;
	public int numObstacles, numPickups;

	public GUIText scoreText;
	int score;

	void Start () {
		score = 0;
		UpdateScore ();

		// Spawn obstacles
		for (int i = 0; i < numObstacles; i++) {
			GameObject obj = (GameObject)Instantiate (obstacle, RandPos (), RandRot ());
			obj.transform.localScale = RandSc ();
		}

		// Spawn pickups
		for (int i = 0; i < numPickups; i++) {
			Vector3 position = new Vector3 (Random.Range (boundary.xMin, boundary.xMax), 0, Random.Range (boundary.xMin, boundary.xMax));
			Instantiate (pickup, position, Quaternion.identity);
		}
	}

	Vector3 RandPos() {
		return new Vector3 (Random.Range (boundary.xMin, boundary.xMax), 5, Random.Range (boundary.xMin, boundary.xMax));
	}
	Quaternion RandRot() {
		return Quaternion.AngleAxis (Random.Range (0, 360), Vector3.up);
	}
	Vector3 RandSc() {
		return new Vector3 (Random.Range (2, 7), 10, Random.Range (5, 12));
	}

	public void AddScore(int val) {
		score += val;
		UpdateScore ();
	}
	void UpdateScore() {
		scoreText.text = "Score: " + score;
	}
}
