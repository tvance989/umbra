using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
	public static Game instance;
	public AudioClip gameOverSound;

	int highScore = 0;
	int score = 0;
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
		//.figure out a better way
		GUI.Label (new Rect (50, 210, 100, 30), "Score: " + score);
		GUI.Label (new Rect (50, 230, 100, 30), "High Score: " + highScore);
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
		Debug.Log ("before score " + score);
		score = 0;
		Debug.Log ("after score " + score);
		if (paused)
			TogglePause ();
		
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
