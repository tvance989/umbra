using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {
	public GameObject pickup;
	public int numPickups;

	int xMin = -40;
	int xMax = 40;
	int zMin = -40;
	int zMax = 40;

	void Start () {
		for (int i = 0; i < numPickups; i++) {
			SpawnRandomPickup ();
		}

		/*for (int i = 0; i < sqrNodes; i++) {
			for (int j = 0; j < sqrNodes; j++) {
				float x = Mathf.Lerp (xMin, xMax, (float)i / (float)sqrNodes) + Random.Range (-jitter, jitter);
				float z = Mathf.Lerp (zMin, zMax, (float)j / (float)sqrNodes) + Random.Range (-jitter, jitter);
				nodes.Add (new Node (new Vector3 (x, 0, z)));
			}
		}*/

		//.InvokeRepeating ("CheckNodes", 0, 0.5f);
	}

	/*bool IsDark() {
		Vector3 effectivePos = transform.position;
		effectivePos.y = 1;
		Vector3 sunToMe = effectivePos - sun.position;
		// If the raycast from the sun to the pickup hits something (obstacle), the pickup is in the dark.
		return Physics.Raycast(sun.position, sunToMe, sunToMe.magnitude);
	}*/

	public void SpawnRandomPickup () {
		Instantiate (pickup, RandPos (), Quaternion.identity);
	}

	Vector3 RandPos () {
		return new Vector3 (Random.Range (xMin, xMax), 0, Random.Range (zMin, zMax));
	}
}
