using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {
	public GameObject healthBar;
	public int maxHealth = 100;
	public Image damageImage;
	public float flashSpeed = 0.5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.2f);

	int curHealth;
	bool damaged;

	RectTransform fullBar;
	float fullBarWidth;

	void Awake () {
		curHealth = maxHealth;

		RectTransform emptyBar = healthBar.GetComponent<RectTransform> ().GetChild (0).GetChild (0).GetComponent<RectTransform> ();
		fullBarWidth = emptyBar.rect.width;
		fullBar = emptyBar.GetChild (0).GetComponent<RectTransform> ();

		UpdateUI ();
	}

	void Update () {
		if(damaged) {
			damageImage.color = flashColour;
		} else {
			// Transition the back to clear.
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		damaged = false;
	}

	public int GetHealth () {
		return curHealth;
	}

	public void GainHealth (int amt) {
		curHealth += amt;
		if (curHealth > maxHealth)
			curHealth = maxHealth;
		UpdateUI ();
	}
	public void GainHealth (float amt) { GainHealth ((int)amt); }

	public void TakeDamage (int amt) {
		damaged = true;
		curHealth -= amt;
		UpdateUI ();
	}
	public void TakeDamage (float amt) { TakeDamage ((int)amt); }

	float GetPercentHealth () {
		return (float)curHealth / (float)maxHealth;
	}

	void UpdateUI () {
		fullBar.sizeDelta = new Vector2 (fullBarWidth * GetPercentHealth (), 0);
	}
}