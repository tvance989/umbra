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

	public float borderThickness = 5f;
	public Color borderColor = Color.black;
	public Color emptyColor = new Color (1, 0, 0, 0.25f);
	public Color fullColor = Color.red;

	RectTransform parentRect, borderRect, emptyRect, fullRect;
	Image borderImg, emptyImg, fullImg;
	Text text;

	void Start () {
		parentRect = GetComponent<RectTransform> ();
		borderRect = parentRect.GetChild (0).GetComponent<RectTransform> ();
		emptyRect = borderRect.GetChild (0).GetComponent<RectTransform> ();
		fullRect = emptyRect.GetChild (0).GetComponent<RectTransform> ();

		borderImg = borderRect.gameObject.GetComponent<Image> ();
		emptyImg = emptyRect.gameObject.GetComponent<Image> ();
		fullImg = fullRect.gameObject.GetComponent<Image> ();

		text = emptyRect.GetChild (1).GetComponent<Text> ();
	}

	public void Init (float max, float val) {
		maxValue = max;
		value = val;
	}
	public void Init (float val) {
		Init (val, val);
	}

	void Update () {
		UpdateUI ();
	}

	void UpdateUI () {
		emptyRect.sizeDelta = new Vector2 (parentRect.rect.width - borderThickness * 2, parentRect.rect.height - borderThickness * 2);

		fullRect.sizeDelta = new Vector2 (emptyRect.rect.width * value / maxValue, 0);

		borderImg.color = borderColor;
		emptyImg.color = emptyColor;
		fullImg.color = fullColor;

		text.text = Mathf.Round(value) + " / " + maxValue;
	}
}
