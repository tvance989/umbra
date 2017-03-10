﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public GameObject sun;
	public PickupSpawner pickupSpawner;//.figure out a better pickup spawn system so the player doesn't have to be responsible
	public float speed;

	Rigidbody rb;
	Health health;
	Game game;

	bool inSun = false;
	float lastShade;
	float nextBurn;
	float damageTime = 0.5f;//.make it public? maybe not bc it exponentially increases based on time in sun. or does it?

	void Start () {
		rb = GetComponent<Rigidbody> ();
		health = GetComponent<Health> ();
		game = GameObject.Find ("Game").GetComponent<Game> ();
	}

	void Update() {
		bool wasInSun = inSun;
		inSun = InDirectSunlight ();

		if (inSun) {
			// If player just left the shade, start timing.
			if (!wasInSun) {
				lastShade = Time.time;//.not being used yet. might use for exponential sunburn.
				nextBurn = Time.time + damageTime;
			}

			// If player has been in the sun too long, take damage.
			if (Time.time > nextBurn) {
				Sunburn ();
			}
		}

		/*if (!playerVisible && playerWasVisible) {
			Debug.Log ("player was visible for " + (Time.time - nextFlare + flareChargeTime) + " seconds");
		}*/
	}
	
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);
		rb.velocity = movement * speed;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Pickup")) {
			PickUpPickup (other.gameObject);
		}
	}

	bool InDirectSunlight () {
		RaycastHit hit;
		Physics.Raycast (transform.position, sun.transform.position - transform.position, out hit);
		return hit.collider.gameObject == sun;
	}

	void Sunburn () {
		LoseHealth (GetSunExposure () * 10); // Fully exposed sunburn := 10 damage.
		nextBurn = Time.time + damageTime;
	}

	public void HandleFlare () {
		LoseHealth (GetSunExposure () * 40); // Fully exposed sun flare := 40 damage.
	}

	float GetSunExposure () {
		Vector3[,] points = GetSunRays ();
		int hits = 0;

		for (int i = 0; i < points.GetLength (0); i++) {
			Vector3 sunPoint = points [i, 0];
			Vector3 playerPoint = points [i, 1];
			if (RayHitsPlayer (sunPoint, playerPoint)) {
				hits++;
				Debug.DrawLine (sunPoint, playerPoint, Color.red, 1);
			} else {
				Debug.DrawLine (sunPoint, playerPoint, Color.blue, 1);
			}
		}

		return (float)hits / (float)points.GetLength (0);
	}

	Vector3[,] GetSunRays () {
		float[] offsets = new float[5] { -1f, -0.5f, 0, 0.5f, 1f };
		Vector3[,] points = new Vector3[offsets.Length, 2];

		float sunRadius = 2.5f;//.bad
		float playerRadius = 1.5f;//.bad

		Vector3 sunToPlayer = transform.position - sun.transform.position;
		Vector3 dirToPlayer = sunToPlayer.normalized;
		Vector3 norm = new Vector3 (dirToPlayer.z, 0, -dirToPlayer.x);

		for (int i = 0; i < offsets.Length; i++) {
			points [i, 0] = sun.transform.position + offsets [i] * sunRadius * norm; // sun point
			points [i, 1] = transform.position + offsets [i] * playerRadius * norm; // player point
		}

		return points;
	}

	bool RayHitsPlayer (Vector3 sunPoint, Vector3 playerPoint) {
		Vector3 sunToPlayer = playerPoint - sunPoint;
		RaycastHit hit;
		Physics.Raycast (sunPoint, sunToPlayer, out hit, sunToPlayer.magnitude);
		return !hit.collider.gameObject.CompareTag ("Obstacle"); // If it doesn't hit an obstacle, assume it hits the player.
	}

	void PickUpPickup (GameObject pickup) {
		Destroy (pickup);
		game.AddScore (10);
		health.GainHealth (10);
		pickupSpawner.SpawnRandomPickup ();//.this doesn't belong in the player class
	}

	void LoseHealth (int val) {
		health.TakeDamage (val);

		if (health.GetHealth () <= 0) {
			game.GameOver ();
		}
	}
	void LoseHealth (float val) {
		LoseHealth ((int)val);
	}
}
