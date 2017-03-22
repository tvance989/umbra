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
