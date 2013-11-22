using UnityEngine;
using System.Collections;
using Leap;

public class HologramControl : MonoBehaviour
{
	private const float bankingSensitivity = 10.0f;
	private const float pitchSensitivity = 75.0f;
	private const float MAX_SHIP_SPEED = 30.0f;
	private const float HAND_SEPARATE = 200.0f;
	private const float HAND_HEIGHT_LEAP = 100.0f;
	
	
	Controller leapControl;
	Frame latestFrame;
	float pitch, yaw, roll, x, y, z;
	float driftSpeed;
	
	
	void Start ()
	{
		try {
			leapControl = new Controller ();
			latestFrame = null;
		} catch {
			Application.Quit ();
		}
	}
	
	void FixedUpdate ()
	{
		latestFrame = leapControl.Frame();
		if (latestFrame.Hands.Count >= 2) {
			Hand leftHand = latestFrame.Hands.Leftmost; 
			Hand rightHand = latestFrame.Hands.Rightmost; 
			
			Vector3 avgPalmForward = (latestFrame.Hands [0].Direction.ToUnity () + latestFrame.Hands [1].Direction.ToUnity ()) / 2;
			Vector3 handDiff = leftHand.PalmPosition.ToUnityScaled() - rightHand.PalmPosition.ToUnityScaled();
			Vector3 newRot = gameObject.transform.localRotation.eulerAngles;
			
			newRot.z = -handDiff.y * bankingSensitivity;
			newRot.x = avgPalmForward.y * - pitchSensitivity;
			transform.localRotation = Quaternion.Slerp (transform.localRotation, Quaternion.Euler (newRot), 0.1f);
		}
	}
}
