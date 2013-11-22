using UnityEngine;
using System.Collections;
using Leap;

public class LeapRightHand : MonoBehaviour {
	
	Controller leapControl;
	Frame latestFrame;
	public Transform defaultPos;
	
	// Use this for initialization
	void Start () {
		try {
			leapControl = new Controller ();
			latestFrame = null;
		} catch {
			Application.Quit ();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		latestFrame = leapControl.Frame();
		if(latestFrame.Hands.Count >= 1){
			Hand leftHand = latestFrame.Hands.Rightmost;
			Vector3 scaleLeap = leftHand.PalmPosition.ToUnityScaled() / 100;
			transform.position = defaultPos.position + scaleLeap;
		}
	}
}