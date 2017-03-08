using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public GameObject sun;
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
			PickUpPickup (other.gameObject);
		}
	}

	public void HandleFlare () {
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

		TakeHits (hits);
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
		Vector3 diff = playerPoint - sunPoint;
		RaycastHit hit;
		Physics.Raycast (sunPoint, diff, out hit, diff.magnitude);
		return !hit.collider.gameObject.CompareTag ("Obstacle");
	}

	void TakeHits (int hits) {
		Debug.Log ("took " + hits + " hits");

		/*for (int i = 0; i < hits; i++) {
			Vector3 pos = transform.position;
			Vector2 rand = Random.insideUnitCircle * 3;
			pos.x += rand.x;
			pos.y = 0;
			pos.z += rand.y;
			Instantiate (game.pickup, pos, Quaternion.identity);
		}*/

		game.AddScore (-hits * 2);
	}

	void PickUpPickup (GameObject pickup) {
		Destroy (pickup);
		game.AddScore (10);
		game.SpawnRandomPickup ();
	}
}
