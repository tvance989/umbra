using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {
	public Text scoreText, highScoreText;

	void Start () {
		scoreText.text = "Score: " + ScoreManager.instance.GetScore ();
		highScoreText.text = "High Score: " + ScoreManager.instance.GetHighScore ();
	}
}
