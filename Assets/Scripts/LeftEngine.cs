using UnityEngine;
using System.Collections;
using Leap;

public class LeftEngine : MonoBehaviour {
	
		
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
		Hand leftHand = latestFrame.Hands.Leftmost;
		Vector3 leftHandPos = leftHand.PalmPosition.ToUnityTranslated();
		Vector3 newRot = gameObject.transform.localRotation.eulerAngles;
		newRot.x = -(leftHandPos.x * 15);
		Debug.Log("New Rot X: " + newRot.x);
		transform.localRotation = Quaternion.Slerp (transform.localRotation, Quaternion.Euler (newRot), 0.3f);	
	}
}
