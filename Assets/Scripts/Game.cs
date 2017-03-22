using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public Text scoreText, highScoreText;

	bool paused = false;
	bool muted = false;

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Space)) {
			TogglePause ();
		}
	}

	void OnGUI () {
		scoreText.text = "Score: " + ScoreManager.instance.GetScore ();
		highScoreText.text = "High Score: " + ScoreManager.instance.GetHighScore ();
	}

	void TogglePause () {
		paused = !paused;

		if (paused)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
	}

	public void GameOver () {
		SceneManager.LoadScene ("GameOver");
	}

	public void MainMenu () {
		SceneManager.LoadScene ("Menu");
	}
	public void Restart () {
		ScoreManager.instance.AddScore (-ScoreManager.instance.GetScore ());//.hack
		
		SceneManager.LoadScene ("Game");
	}
	public void Quit () {
		Application.Quit ();
	}

	public void ToggleMute () {
		Debug.Log ("toggling mute");
		muted = !muted;
		if (muted)
			AudioListener.volume = 0;
		else
			AudioListener.volume = 1;
	}
}
