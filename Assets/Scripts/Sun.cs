﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {public GameObject player;
	public Pathfinder pathfinder;
	public float flareChargeTime; // seconds it takes to charge a flare
	public GameObject sunFlareParticles;
	public AudioClip flareSound;

	Rigidbody rb;
	Vehicle vehicle;
	List<Vector3> path;
	Vector3 goal;
	new AudioSource audio;

	bool playerVisible;
	float nextFlare;
	int flareMin = 5;
	int flareMax = 15;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		vehicle = GetComponent<Vehicle> ();
		path = new List<Vector3> ();
		SetGoalToPlayer ();
		nextFlare = Time.time + Random.Range (flareMin, flareMax);
		audio = GetComponent<AudioSource> ();
	}

	void Update () {
		if (Time.time > nextFlare) {
			FlareUp ();
		}

		playerVisible = CanSeePlayer ();

		// Find path and/or set goal.
		if (playerVisible) {
			path.Clear ();
			SetGoalToPlayer ();
		} else {
			bool repath = false;

			if (path.Count > 0) {
				Vector3 toEndOfPath = path [path.Count - 1] - transform.position;
				if (toEndOfPath.sqrMagnitude < 5 * 5) {//.arbitrary
					// If the sun has reached the end of the path and still doesn't see the player, give the sun a flash of clairvoyance.
					SetGoalToPlayer ();
					repath = true;
				}
			} else {
				repath = true;
			}

			if (repath)
				path = pathfinder.GetPath (transform.position, goal);
		}
	}
	
	void FixedUpdate () {
		Steer ();
	}

	void Steer () {
		Vector3 force = Vector3.zero;

		if (playerVisible) {
			// Arrive at the player's position.
			force += vehicle.Arrive (goal);
		} else if (path.Count > 2) {//.arbitrary
			force += vehicle.FollowPath (path, 1.5f) * 3f;
		}

		force += vehicle.AvoidObstacles () * 1.5f;

		Debug.DrawLine (transform.position, transform.position + force);
		vehicle.ApplyForce (force);
	}

	void FlareUp () {
		audio.PlayOneShot (flareSound, 0.5f);
		GameObject flare = Instantiate (sunFlareParticles, transform.position, Quaternion.identity);
		flare.transform.SetParent (transform);
		Destroy (flare, 5f);
		player.SendMessage ("HandleFlare");
		nextFlare = Time.time + Random.Range (flareMin, flareMax);
	}

	void SetGoalToPlayer () {
		goal = player.transform.position;
		goal.y = transform.position.y;
	}

	void OnCollisionEnter (Collision coll) {
		// Bounce off of obstacles.
		if (coll.gameObject.CompareTag ("Obstacle")) {
			Vector3 away = transform.position - coll.contacts [0].point;
			away *= 2;
			rb.AddForce (away, ForceMode.Impulse);
		}
	}

	bool CanSeePlayer () {
		RaycastHit hit;
		Physics.Raycast (transform.position, player.transform.position - transform.position, out hit);
		return hit.collider.gameObject == player;
	}

	void OnDrawGizmos () {
		if (path == null)
			return;
		for (int i = 0; i < path.Count; i++) {
			Gizmos.color = Color.Lerp (Color.blue, Color.green, (float)i / path.Count);
			Gizmos.DrawSphere (path [i], 0.5f);
		}
		Gizmos.color = Color.red;
		if (playerVisible)
			Gizmos.DrawLine (transform.position, player.transform.position);
	}
}
