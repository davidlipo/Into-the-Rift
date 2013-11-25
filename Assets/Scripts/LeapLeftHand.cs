using UnityEngine;
using System.Collections;
using Leap;

public class LeapLeftHand : MonoBehaviour {
	
	Controller leapControl;
	Frame latestFrame;
	Vector3 prevLeftHand;
	
	// Use this for initialization
	void Start () {
		try {
			leapControl = new Controller ();
			latestFrame = null;
			prevLeftHand = new Vector3(0,0,0);
		} catch {
			Application.Quit ();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		latestFrame = leapControl.Frame();

	}
}
