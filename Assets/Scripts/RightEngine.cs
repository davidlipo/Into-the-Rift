using UnityEngine;
using System.Collections;
using Leap;

public class RightEngine : MonoBehaviour {
	
		
	Controller leapControl;
	Frame latestFrame;
	
	// Use this for initialization
	void Start ()
	{
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
		Hand rightHand = latestFrame.Hands.Rightmost;
		Vector3 rightHandPos = rightHand.PalmPosition.ToUnityTranslated();
		Vector3 newRot = gameObject.transform.localRotation.eulerAngles;
		newRot.x = rightHandPos.x * -25;
		Debug.Log("New Rot X: " + newRot.x);
		transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (newRot), 0.1f);	
	}
}

