using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour {
	void Start () {
		GetComponent<Rigidbody> ().velocity = Vector3.forward * 2;
		Destroy (gameObject, 1);
	}
}
