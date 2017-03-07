using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float speed;

	Rigidbody rb;
	Game game;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		game = GameObject.Find ("Game").GetComponent<Game> ();
	}
	
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);
		rb.velocity = movement * speed;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Pickup")) {
			game.AddScore (1);
			game.SpawnRandomPickup ();
			Destroy (other.gameObject);
		}
	}
}
