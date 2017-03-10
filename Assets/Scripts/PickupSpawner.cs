using System.Collections;
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
	}

	public void SpawnRandomPickup () {
		Instantiate (pickup, RandPos (), Quaternion.identity);
	}

	Vector3 RandPos () {
		return new Vector3 (Random.Range (xMin, xMax), 0, Random.Range (zMin, zMax));
	}
}
