using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Bar : MonoBehaviour {
	public float maxValue = 100f;

	[SerializeField]
	float _value = 100f;
	public float value {
		get { return _value; }
		set { _value = Mathf.Clamp (value, 0, maxValue); }
	}

	public Color borderColor = Color.black;
	public Color emptyColor = new Color (1f, 0f, 0f, 0.25f);
	public Color fullColor = Color.red;

	RectTransform borderRect, emptyRect, fullRect;
	Image borderImg, emptyImg, fullImg;
	Text text;

	void Start () {
		_value = maxValue;

		borderRect = gameObject.GetComponent<RectTransform> ().GetChild (0).GetComponent<RectTransform> ();
		emptyRect = borderRect.GetChild (0).GetComponent<RectTransform> ();
		fullRect = emptyRect.GetChild (0).GetComponent<RectTransform> ();

		borderImg = borderRect.gameObject.GetComponent<Image> ();
		emptyImg = emptyRect.gameObject.GetComponent<Image> ();
		fullImg = fullRect.gameObject.GetComponent<Image> ();

		text = emptyRect.GetChild (1).GetComponent<Text> ();
	}

	public void Init (float max, float? cur = null) {
		maxValue = max;
		value = cur ?? max;
	}

	void Update () {
		UpdateUI ();
	}

	void UpdateUI () {
		fullRect.sizeDelta = new Vector2 (emptyRect.rect.width * _value / maxValue, 0);

		borderImg.color = borderColor;
		emptyImg.color = emptyColor;
		fullImg.color = fullColor;

		text.text = Mathf.Round(_value) + " / " + maxValue;
	}
}
