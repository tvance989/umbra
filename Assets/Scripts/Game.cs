using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
	public static Game instance;

	int highScore = 0;
	int score = 0;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	void OnGUI () {
		GUI.Label (new Rect (50, 70, 100, 30), "Score: " + score);
		GUI.Label (new Rect (50, 90, 100, 30), "High Score: " + highScore);
	}

	public void AddScore (int val) {
		score += val;

		if (score < 0)
			score = 0;
	}

	public void GameOver () {
		if (score > highScore)
			highScore = score;

		score = 0;
		
		SceneManager.LoadScene ("GameOver");
	}
}
