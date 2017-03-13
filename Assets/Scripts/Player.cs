using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public GameObject sun;
	public PickupSpawner pickupSpawner;//.figure out a better pickup spawn system so the player doesn't have to be responsible
	public float speed;
	public AudioClip pickupSound;
	public AudioClip sizzleSound;
	public AudioClip flareHitSound;
	public GameObject damageText;

	Rigidbody rb;
	Health health;
	Game game;
	AudioSource audio;

	bool inSun = false;
	float lastShade;
	float nextBurn;
	float damageTime = 0.5f;//.make it public? maybe not bc it exponentially increases based on time in sun. or does it?

	int level = 1;
	float minSunburnDamage = 1;
	float maxSunburnDamage = 50;
	float minFlareDamage = 20;
	float maxFlareDamage = 100;
	float sunburnDamage;
	float flareDamage;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		health = GetComponent<Health> ();
		game = GameObject.Find ("Game").GetComponent<Game> ();
		audio = GetComponent<AudioSource> ();

		sunburnDamage = minSunburnDamage;
		flareDamage = minFlareDamage;
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
		audio.PlayOneShot (sizzleSound, 0.5f);
		LoseHealth (GetSunExposure () * sunburnDamage);
		nextBurn = Time.time + damageTime;
	}

	public void HandleFlare () {
		float exposure = GetSunExposure ();
		if (exposure > 0) {
			audio.PlayOneShot (flareHitSound, 0.5f);
			LoseHealth (exposure * flareDamage);
		}
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
		audio.PlayOneShot (pickupSound, 0.1f);
		Destroy (pickup);
		game.AddScore (10);
		health.GainHealth (10);

		pickupSpawner.SpawnRandomPickup ();//.this doesn't belong in the player class

		//.figure out better way to increase difficulty
		if (game.GetScore () >= level * 100) {
			level++;

			sunburnDamage = Mathf.Lerp (minSunburnDamage, maxSunburnDamage, (float)game.GetScore() / 1000f);
			flareDamage = Mathf.Lerp (minFlareDamage, maxFlareDamage, (float)game.GetScore() / 1000f);

			Debug.Log ("Level " + level + "; sunburn dmg " + sunburnDamage + "; flare dmg " + flareDamage);
		}
	}

	void LoseHealth (int val) {
		health.TakeDamage (val);

		GameObject obj = (GameObject)Instantiate (damageText, transform.position + Vector3.forward * 3, Quaternion.Euler (new Vector3 (90, 0, 0)));
		obj.GetComponent<TextMesh> ().text = "-" + val;

		if (health.GetHealth () <= 0) {
			game.GameOver ();
		}
	}
	void LoseHealth (float val) {
		LoseHealth ((int)val);
	}
}
