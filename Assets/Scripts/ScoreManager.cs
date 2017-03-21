using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
	private static ScoreManager _instance;

	public static ScoreManager instance {
		get {
			if (_instance == null) {
				var obj = new GameObject ("ScoreManager");
				obj.AddComponent<ScoreManager> ();
				_instance = obj.GetComponent<ScoreManager> ();
			}
			return _instance;
		}
	}

	int score, highScore;

	void Awake () {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (_instance != this) {
			Destroy (gameObject);
		}
	}

	public void AddScore (int val) {
		score += val;

		if (score < 0)
			score = 0;

		if (score > highScore)
			highScore = score;
	}

	public int GetScore () {
		return score;
	}

	public int GetHighScore () {
		return highScore;
	}
}
