using UnityEngine;
using System.Collections;

public class TextSequence : MonoBehaviour {
	
	float[] TEXT_DURATIONS = new float[] {3.0f, 3.0f, 3.0f, 3.0f};
	string[] TEXT_STRINGS = new string[] {"What", "does", "the fox say?", "#WhatDoesTheFoxSay"};
	
	float startTime;
	
	void Start () {
		startTime = Time.time;
	}
	
	void OnGUI() {
		GUI.Label(new Rect(0, 0, Screen.width, Screen.height), currentText());
	}
	
	void Update () {
		
	}
	
	string currentText() {
		float duration = 0;
		float timeDiff = Time.time - startTime;
		for (int i = 0; i < TEXT_STRINGS.Length; i++) {
			if (timeDiff > duration && timeDiff < TEXT_DURATIONS[i]+duration) {
				return TEXT_STRINGS[i];
			}
			duration += TEXT_DURATIONS[i];
		}
		
		return "";
	}
}
