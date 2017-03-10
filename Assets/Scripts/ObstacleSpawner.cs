using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {
	public GameObject obstacle;

	int sqObstacles = 4;
	int xMin = -35;
	int xMax = 35;
	int zMin = -35;
	int zMax = 35;
	int jitter = 3;

	void Start () {
		for (int i = 0; i < sqObstacles; i++) {
			for (int j = 0; j < sqObstacles; j++) {
				float x = Mathf.Lerp (xMin, xMax, (float)i / (float)(sqObstacles - 1)) + Random.Range (-jitter, jitter);
				float z = Mathf.Lerp (zMin, zMax, (float)j / (float)(sqObstacles - 1)) + Random.Range (-jitter, jitter);
				GameObject obj = (GameObject)Instantiate (obstacle, new Vector3 (x, 5, z), RandRot ());
				obj.transform.localScale = RandSc ();
			}
		}
	}

	Quaternion RandRot () {
		return Quaternion.AngleAxis (Random.Range (0, 360), Vector3.up);
	}

	Vector3 RandSc () {
		return new Vector3 (Random.Range (2, 7), 10, Random.Range (5, 12));
	}
}
