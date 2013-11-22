using UnityEngine;
using System.Collections;
using Leap;

public class CameraTorque : MonoBehaviour {
	
	public float bankingSensitivity;
	public float pitchSensitivity;
	public float yawSensitivity;
	
	Controller leapControl;
	Frame latestFrame;
	
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
	void Update () {
		latestFrame = leapControl.Frame();
		if (latestFrame.Hands.Count == 2) {
			Hand leftHand = latestFrame.Hands.Leftmost;
			Hand rightHand = latestFrame.Hands.Rightmost;
			
			Vector3 avgPalmForward = (latestFrame.Hands [0].Direction.ToUnity() + latestFrame.Hands [1].Direction.ToUnity ()) / 2;
			Vector3 handDiff = leftHand.PalmPosition.ToUnityScaled() - rightHand.PalmPosition.ToUnityScaled();
			rigidbody.AddTorque(transform.forward * (-handDiff.y *bankingSensitivity));
			rigidbody.AddTorque(transform.up * (handDiff.z * yawSensitivity));
			rigidbody.AddTorque(transform.right * (avgPalmForward.y * pitchSensitivity));			
		}
	}
}
