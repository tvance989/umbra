using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
	public float timeToGrow;

	Transform sun;
	float lastLight;

	void Start () {
		sun = GameObject.Find ("Sun").transform;
		lastLight = Time.time;
		// Start out of view (below the field).
		Vector3 pos = transform.position;
		pos.y = -10;
		transform.position = pos;
	}

	void Update () {
		Vector3 pos = transform.position;

		Debug.DrawLine(transform.position, sun.position, Color.blue);
		if (IsDark()) {
			if (Time.time - lastLight > timeToGrow) {
				pos.y = 0;//.might need to change if prefab changes
			}
		} else {
			lastLight = Time.time;
			pos.y = -10;
		}

		transform.position = pos;
	}

	bool IsDark() {
		Vector3 effectivePos = transform.position;
		effectivePos.y = 1;
		Vector3 sunToMe = effectivePos - sun.position;
		// If the raycast from the sun to the pickup hits something (obstacle), the pickup is in the dark.
		return Physics.Raycast(sun.position, sunToMe, sunToMe.magnitude);
	}
}
