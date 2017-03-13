﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
	public static Game instance;

	int highScore = 0;
	int score = 0;
	bool paused = false;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			paused = !paused;
			if (paused)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}
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

	public int GetScore () {
		return score;
	}

	public void GameOver () {
		Debug.Log ("GAME OVER! Score: " + score);

		if (score > highScore)
			highScore = score;
		score = 0;

		SceneManager.LoadScene ("GameOver");
	}

	public void Restart () {
		SceneManager.LoadScene ("Main");
	}
}
