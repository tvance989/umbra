using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}

public class Game : MonoBehaviour {
	public Boundary boundary;
	public GameObject obstacle, pickup;
	public int numPickups;
	//public float pickupSpawnRate; // pickups per second
	public GUIText scoreText;

	int score;

	void Start () {
		score = 0;
		UpdateScore ();

		// Spawn obstacles
		int sqObstacles = 4;
		int jitter = 3;
		for (int i = 0; i < sqObstacles; i++) {
			for (int j = 0; j < sqObstacles; j++) {
				float x = Mathf.Lerp (boundary.xMin + 5, boundary.xMax - 5, (float)i / (sqObstacles - 1));
				float z = Mathf.Lerp (boundary.zMin + 5, boundary.zMax - 5, (float)j / (sqObstacles - 1));
				x += Random.Range (-jitter, jitter);
				z += Random.Range (-jitter, jitter);
				GameObject obj = (GameObject)Instantiate (obstacle, new Vector3 (x, 5, z), RandRot ());
				obj.transform.localScale = RandSc ();
			}
		}

		//.make it continuous
		// Spawn pickups
		for (int i = 0; i < numPickups; i++) {
			SpawnRandomPickup ();
		}
	}

	void Update () {
		/*if (Random.value < Time.deltaTime / pickupSpawnRate) {
			Debug.Log ("spawn pickup! " + Random.value);
		}*/
	}

	void OnGUI () {
		if (GUILayout.Button ("Restart")) {
			SceneManager.LoadScene ("Scene1");
		}
	}

	public void SpawnRandomPickup () {
		Instantiate (pickup, RandPos (), Quaternion.identity);
	}

	Vector3 RandPos () {
		return new Vector3 (Random.Range (boundary.xMin, boundary.xMax), 0, Random.Range (boundary.xMin, boundary.xMax));
	}
	Quaternion RandRot () {
		return Quaternion.AngleAxis (Random.Range (0, 360), Vector3.up);
	}
	Vector3 RandSc () {
		return new Vector3 (Random.Range (2, 7), 10, Random.Range (5, 12));
	}

	public void AddScore (int val) {
		score += val;
		UpdateScore ();
	}
	void UpdateScore () {
		scoreText.text = "Score: " + score;
	}
}
