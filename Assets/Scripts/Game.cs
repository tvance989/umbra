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
		//.access ScoreManager.instance.score etc

		//.awkward bc game exists outside of main scene
		GameObject temp;
		if (scoreText == null) {
			temp = GameObject.Find ("ScoreText");
			if (temp != null)
				scoreText = temp.GetComponent<Text> ();
		}
		if (highScoreText == null) {
			temp = GameObject.Find ("HighScoreText");
			if (temp != null)
				highScoreText = temp.GetComponent<Text> ();
		}

		if (scoreText != null)
			scoreText.text = "Score: " + ScoreManager.instance.GetScore ();
		if (highScoreText != null)
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
		Debug.Log ("GAME OVER! Score: " + ScoreManager.instance.GetScore ());

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
