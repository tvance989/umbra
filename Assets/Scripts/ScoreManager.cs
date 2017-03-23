using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
	public static ScoreManager instance {
		get {
			if (_instance == null) {
				var obj = new GameObject ("ScoreManager");
				_instance = obj.AddComponent<ScoreManager> () as ScoreManager;
			}
			return _instance;
		}
	}

	private static ScoreManager _instance;

	int score, highScore;
	string initials;

	void Awake () {
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	void Start () {
		LoadHighScore ();
	}

	void LoadHighScore () {
		highScore = PlayerPrefs.GetInt ("highscore", 0);
		initials = PlayerPrefs.GetString ("initials", "AAA");
	}

	public void ClearHighScore () {
		SetHighScore (0, "AAA");
	}

	void SetHighScore (int num, string inits) {
		PlayerPrefs.SetInt ("highscore", num);
		PlayerPrefs.SetString ("initials", inits);

		LoadHighScore ();
	}

	public void AddScore (int val) {
		score += val;

		if (score < 0)
			score = 0;
	}

	public int GetScore () {
		return score;
	}

	public int GetHighScore () {
		return highScore;
	}

	public string GetHighScoreInitials () {
		return initials;
	}

	public void SubmitScore(string inits) {
		if (score > highScore)
			SetHighScore (score, inits);
	}
}
