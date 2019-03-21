﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
	private Text texttoFade;
	public float speed = 1.0f;
	private float time = 0f;
	private Color textColor;
	public float changeAmount = 0.4f;

	// Use this for initialization
	void Start () {
		texttoFade = GetComponent<Text> ();
		textColor = texttoFade.color;
	}

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime * speed;
		textColor.a = 1 - Mathf.PingPong (time, changeAmount);
		texttoFade.color = textColor;

	}

}
