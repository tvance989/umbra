using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public static Game instance;
	public AudioClip gameOverSound;
	public Text scoreText, highScoreText;

	int score = 0;
	int highScore = 0;
	bool paused;
	bool muted;
	new AudioSource audio;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	void Start () {
		audio = GetComponent<AudioSource> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Space)) {
			TogglePause ();
		}
	}

	void OnGUI () {
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
			scoreText.text = "Score: " + score;
		if (highScoreText != null)
			highScoreText.text = "High Score: " + highScore;
	}

	void TogglePause () {
		paused = !paused;

		if (paused)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
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
		audio.PlayOneShot (gameOverSound, 0.5f);
		Debug.Log ("GAME OVER! Score: " + score);

		if (score > highScore)
			highScore = score;

		SceneManager.LoadScene ("GameOver");
	}

	public void MainMenu () {
		SceneManager.LoadScene ("Menu");
	}
	public void Restart () {
		//.does this belong somewhere else?
		instance.score = 0;
		if (instance.paused)
			instance.TogglePause ();
		
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
