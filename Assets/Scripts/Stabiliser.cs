using UnityEngine;
using System.Collections;
using Leap;

public class Stabiliser : MonoBehaviour {
	
	Controller leapControl;
	float startTime;
	bool stabiliseTopHit;
	bool stabilising;
	
	public float rotRange;
	public float damping;
	
	// Use this for initialization
	void Start () {
		try {
			leapControl = new Controller ();
			stabiliseTopHit = false;
			stabilising = false;
		} catch {
			Application.Quit ();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Frame latestFrame = leapControl.Frame();
		if (latestFrame.Hands.Count == 2) {
			Hand leftHand = latestFrame.Hands.Leftmost;
			Hand rightHand = latestFrame.Hands.Rightmost;
			if(checkStabiliseGesture(leftHand, rightHand)){
				stabilising = true;
			}
		}
		if(stabilising){
			GetComponent<LeapControl>().driftSpeed = 0;
			GetComponent<LeapControl>().enabled = false;
			stabiliseShip();
		}else{
			GetComponent<LeapControl>().enabled = true;
		}
	}
	
	private bool checkStabiliseGesture(Hand leftHand, Hand rightHand){
		float leftHandHeight = leftHand.PalmPosition.y;
		float rightHandHeight = rightHand.PalmPosition.y;
		float averageHandHeight = (leftHandHeight + rightHandHeight) / 2;
		
		if(!stabiliseTopHit){
			if(averageHandHeight > 175){
				stabiliseTopHit = true;
				startTime = Time.time;
				return false;
			}
		}else{
			if(averageHandHeight <= 120){
				float timeDiff = Time.time - startTime;
				if(timeDiff < 0.9){
					Debug.Log("Time Diff: " + timeDiff);
					stabiliseTopHit = false;
					return true;
				}else{
					stabiliseTopHit = false;
					Debug.Log ("Out of Time");
				}
			}
		}
		return false;
	}
	
	private void stabiliseShip(){
		Vector3 oldRot = rigidbody.angularVelocity;
		
		bool inRangeX = (oldRot.x > -rotRange && oldRot.x < rotRange);
		bool inRangeY = (oldRot.y > -rotRange && oldRot.y < rotRange);
		bool inRangeZ = (oldRot.z > -rotRange && oldRot.z < rotRange);
		
		if((inRangeX == true)  && (inRangeY == true) && (inRangeZ == true)){
			Debug.Log("In here");
			rigidbody.angularVelocity = new Vector3(0,0,0);
			stabilising = false;
		}else{
			Debug.Log("In here");
			Vector3 newRot = new Vector3(oldRot.x * damping, oldRot.y * damping, oldRot.z * damping);			
			rigidbody.angularVelocity = newRot;
		}
	
		//transform.localRotation = Quaternion.Slerp (transform.localRotation, Quaternion.Euler (new Vector3(0,0,0)), 0.1f);
	}
}
