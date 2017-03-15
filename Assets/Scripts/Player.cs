using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public GameObject sun;
	public PickupSpawner pickupSpawner;//.figure out a better pickup spawn system so the player doesn't have to be responsible
	public float speed;
	public AudioClip pickupSound;
	public AudioClip sizzleSound;
	public AudioClip flareHitSound;
	public GameObject damageText;
	public float maxHealth;
	public Image damageImage;
	public Bar healthBar;
	public Bar levelBar;

	Rigidbody rb;
	Game game;
	new AudioSource audio;

	bool inSun = false;
	//float lastShade;
	float nextBurn;
	float damageTime = 0.5f;
	Color flashColour = new Color(1f, 1f, 0.5f, 0.2f);
	bool flashDamage;

	int level = 1;
	float minSunburnDamage = 1;
	float maxSunburnDamage = 50;
	float minFlareDamage = 20;
	float maxFlareDamage = 100;
	float sunburnDamage;
	float flareDamage;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		game = GameObject.Find ("Game").GetComponent<Game> ();
		audio = GetComponent<AudioSource> ();

		healthBar.Init (maxHealth);
		levelBar.Init (100, 0);
		levelBar.value = 0;//.why is this necessary?

		sunburnDamage = minSunburnDamage;
		flareDamage = minFlareDamage;
	}

	void Update() {
		bool wasInSun = inSun;
		inSun = InDirectSunlight ();

		if (inSun) {
			// If player just left the shade, start timing.
			if (!wasInSun) {
				//lastShade = Time.time;//.not being used yet. might use for exponential sunburn.
				nextBurn = Time.time + damageTime;
			}

			// If player has been in the sun too long, take damage.
			if (Time.time > nextBurn) {
				Sunburn ();
			}
		}

		if(flashDamage) {
			damageImage.color = flashColour;
		} else {
			// Transition the back to clear.
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, damageTime * Time.deltaTime);
		}

		flashDamage = false;

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

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.name == "Sun") {
			game.GameOver ();
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
		healthBar.value += 10;
		levelBar.value += 10;

		pickupSpawner.SpawnRandomPickup ();//.this doesn't belong in the player class

		//.figure out better way to increase difficulty
		if (game.GetScore () >= level * 100) {
			level++;
			levelBar.value = 0;

			sunburnDamage = Mathf.Lerp (minSunburnDamage, maxSunburnDamage, (float)game.GetScore() / 1000f);
			flareDamage = Mathf.Lerp (minFlareDamage, maxFlareDamage, (float)game.GetScore() / 1000f);

			Debug.Log ("Level " + level + "; sunburn dmg " + sunburnDamage + "; flare dmg " + flareDamage);
		}
	}

	void LoseHealth (float val) {
		if (val > 0)
			flashDamage = true;
		
		healthBar.value -= val;

		GameObject obj = (GameObject)Instantiate (damageText, transform.position + Vector3.forward * 3, Quaternion.Euler (new Vector3 (90, 0, 0)));
		obj.GetComponent<TextMesh> ().text = "-" + val;

		if (healthBar.value <= 0) {
			game.GameOver ();
		}
	}
}
