using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {
	public Text scoreText, highScoreText;
	public InputField initialsInput;
	public Button submitButton;

	void Start () {
		initialsInput.onValueChanged.AddListener (delegate {
			ForceUpper();
		});
		initialsInput.onValidateInput += delegate(string text, int charIndex, char addedChar) {
			return ValidateInitials (addedChar);
		};

		initialsInput.Select ();
	}

	void OnGUI () {
		scoreText.text = "Score: " + ScoreManager.instance.GetScore ();
		highScoreText.text = "High Score: " + ScoreManager.instance.GetHighScore () + " (" + ScoreManager.instance.GetHighScoreInitials () + ")";
	}

	public void SubmitScore () {
		ScoreManager.instance.SubmitScore (initialsInput.text);
		Destroy (submitButton.gameObject);
	}

	public void ClearHighScore () {
		ScoreManager.instance.ClearHighScore ();
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
