using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {
	public Text scoreText, highScoreText;
	public InputField initialsInput;

	void Start () {
		scoreText.text = "Score: " + ScoreManager.instance.GetScore ();
		highScoreText.text = "High Score: " + ScoreManager.instance.GetHighScore ();

		initialsInput.onValueChanged.AddListener (delegate {
			ForceUpper();
		});
		initialsInput.onValidateInput += delegate(string text, int charIndex, char addedChar) {
			return ValidateInitials (addedChar);
		};

		initialsInput.Select ();
	}

	void ForceUpper () {
		if (initialsInput.text != initialsInput.text.ToUpper ())
			initialsInput.text = initialsInput.text.ToUpper ();
	}

	char ValidateInitials (char c) {
		// If it's not a letter, return empty char.
		return System.Char.IsLetter(c) ? c : '\0';
	}
}
