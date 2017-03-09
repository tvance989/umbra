using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {
	public int maxHealth = 100;
	public Slider healthSlider;
	public Image damageImage;
	public float flashSpeed = 0.5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

	int currentHealth;
	bool damaged;

	void Awake () {
		healthSlider.value = healthSlider.maxValue = currentHealth = maxHealth;
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

	public void GainHealth (int amt) {
		currentHealth += amt;
		if (currentHealth > maxHealth)
			currentHealth = maxHealth;
		healthSlider.value = currentHealth;
	}

	public void TakeDamage (int amt) {
		damaged = true;
		currentHealth -= amt;
		healthSlider.value = currentHealth;
	}

	public int GetHealth () {
		return currentHealth;
	}
}