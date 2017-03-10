using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
	public GUIText scoreText;

	int score;

	void Start () {
		score = 0;
		UpdateScore ();
	}

	public void AddScore (int val) {
		score += val;
		if (score < 0)
			score = 0;
		UpdateScore ();
	}
	void UpdateScore () {
		scoreText.text = "Score:\n" + score;
	}

	public void GameOver () {
		SceneManager.LoadScene ("GameOver");
	}
}
