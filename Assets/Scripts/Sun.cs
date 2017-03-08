﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {
	public GameObject player;
	public Pathfinder pathfinder;
	public float flareChargeTime; // seconds it takes to charge a flare

	Rigidbody rb;
	Vehicle vehicle;
	List<Vector3> path;
	Vector3 goal;

	bool playerVisible;
	float nextFlare = Mathf.Infinity;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		vehicle = GetComponent<Vehicle> ();
		path = new List<Vector3> ();
		SetGoalToPlayer ();
	}

	void Update () {
		bool playerWasVisible = playerVisible;
		playerVisible = CanSeePlayer ();

		if (playerVisible) {
			// If visible now but not before, start timing.
			if (!playerWasVisible)
				nextFlare = Time.time + flareChargeTime;

			// If the sun has had time to charge, then do some damage!
			if (Time.time > nextFlare) {
				Debug.Log ("FLAME ON!!! " + Time.time);
				FlareUp ();
			}
		}

		if (!playerVisible && playerWasVisible) {
			Debug.Log ("player was visible for " + (Time.time - nextFlare + flareChargeTime) + " seconds");
		}

		// Find path and/or set goal.
		if (playerVisible) {
			path.Clear ();
			SetGoalToPlayer ();
		} else {
			path = pathfinder.GetPath (transform.position, goal);

			// If the sun has reached the end of the path and still doesn't see the player, give the sun a flash of clairvoyance.
			if (path.Count <= 1) {//.arbitrary
				SetGoalToPlayer ();
			}
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
		} else if (path.Count > 1) {//.arbitrary
			//.figure out path following steering behavior
			// Seek the first point past the start node.
			Vector3 seek = path [1];//.arbitrary
			seek.y = transform.position.y;
			force += vehicle.Seek (seek);
		}

		force += vehicle.AvoidObstacles () * 1.5f;

		Debug.DrawLine (transform.position, transform.position + force);
		vehicle.ApplyForce (force);
	}

	void FlareUp () {
		player.SendMessage ("HandleFlare");
		nextFlare = Time.time + flareChargeTime;
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
