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
		GameObject ship = GameObject.Find("ShipModel");;
		if(latestFrame.Hands.Count >= 2){
			Hand leftHand = latestFrame.Hands.Leftmost;
			Vector3 leftHandPos = leftHand.PalmPosition.ToUnity();
			float diffX = leftHandPos.x - prevLeftHand.x;
			float diffY = leftHandPos.y - prevLeftHand.y;
			float diffZ = leftHandPos.z - prevLeftHand.z;
			Debug.Log("Diff X: " + diffX + "Diff Y: " + diffY + "Diff Z: " + diffZ);
			if(diffX > 1 || diffY > 1 || diffY > 1 ){
				transform.localPosition =  transform.localPosition + (leftHandPos / 1000);
				prevLeftHand = leftHandPos;
			}
			//Debug.Log("Local Position: " + transform.localPosition.ToString());
		}else{
			//transform.rigidbody.angularVelocity = transform.rigidbody.angularVelocity;
			//transform.rigidbody.velocity = transform.rigidbody.velocity;
			//Debug.Log("Velocity: " + transform.rigidbody.velocity);
			//Debug.Log("Angular Velocity: " + transform.rigidbody.angularVelocity);
		}
	}
}
