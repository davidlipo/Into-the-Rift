using UnityEngine;
using System.Collections;
using Leap;

public class LeapControl : MonoBehaviour
{
	public float bankingSensitivity;
	public float pitchSensitivity;
	public float yawSensitivity;
	public float MAX_SHIP_SPEED;
	public float driftSpeed;
	public bool launched;
	private const float HAND_SEPARATE = 200.0f;
	private const float HAND_HEIGHT_LEAP = 70.0f;
	private Vector3 newRot = new Vector3();
	
	Controller leapControl;
	Frame latestFrame;
	float pitch, yaw, roll, x, y, z;
	string info;
	
	void Start ()
	{
		try {
			leapControl = new Controller ();
			latestFrame = null;
			info = "Connected";
		} catch {
			Application.Quit ();
		}
	}
	
	void OnGUI ()
	{
		GUI.Box (new Rect (0, 0, 700, 150), info);
	}
	
	void FixedUpdate ()
	{
		latestFrame = leapControl.Frame();
		info = "";
		rigidbody.angularVelocity = new Vector3(0,0,0);
		if(launched){
				if (latestFrame.Hands.Count == 2) {
					Hand leftHand = latestFrame.Hands.Leftmost;
					Hand rightHand = latestFrame.Hands.Rightmost;

					setLeftHand(leftHand);
					setRightHand(rightHand);
					//if hands are too close together
					float handXDiff = Mathf.Abs(leftHand.PalmPosition.x - rightHand.PalmPosition.x); 
					if(handXDiff < HAND_SEPARATE){
						info += "Hands are too close together\n";
					}
				
					//if hands are too close to the leap
					float[] handHeights = checkHandHeight(leftHand, rightHand);
					info += "Left Hand: " + handHeights[0] + "\n"; 
					info += "Right Hand: " + handHeights[1] + "\n"; 
				
					Vector3 avgPalmForward = (latestFrame.Hands [0].Direction.ToUnity() + latestFrame.Hands [1].Direction.ToUnity ()) / 2;
					Vector3 handDiff = leftHand.PalmPosition.ToUnityScaled() - rightHand.PalmPosition.ToUnityScaled();
					rigidbody.AddTorque(transform.forward * (-handDiff.y *bankingSensitivity));
					rigidbody.AddTorque(transform.up * (handDiff.z * yawSensitivity));
					rigidbody.AddTorque(transform.right * (avgPalmForward.y * pitchSensitivity));			
					float averageZ = (leftHand.PalmPosition.z + rightHand.PalmPosition.z) / 2;
						
					if(averageZ < -25){
						if(driftSpeed < MAX_SHIP_SPEED){	
							driftSpeed++;
						}
					}else if(averageZ > 30){
						if(driftSpeed > -MAX_SHIP_SPEED){
							driftSpeed--;
						}
					}
					
					info += ", Drift Speed: " + driftSpeed;	
					transform.rigidbody.velocity = transform.forward * driftSpeed;
			}else{
				transform.rigidbody.velocity = transform.forward * driftSpeed;
			}
		}else{
			if(latestFrame.Hands.Count >= 2){
				Hand leftHand = latestFrame.Hands.Leftmost;
				Hand rightHand = latestFrame.Hands.Rightmost;
				
				setLeftHand(leftHand);
				setRightHand(rightHand);	
			}
		}
	}

	private void setLeftHand(Hand leftHand){
		Vector3 defaultPos = GameObject.Find("LeftHandDefault").GetComponent<Transform>().localPosition;
		Vector3 leftHandPos = leftHand.PalmPosition.ToUnityTranslated() / 100;
		leftHandPos.y -= 0.03f;
		leftHandPos.x += 0.01f;
		GameObject.Find("LeftHand").GetComponent<Transform>().localPosition = defaultPos + leftHandPos;
	}

	private void setRightHand(Hand rightHand){
		Vector3 defaultPos = GameObject.Find("RightHandDefault").GetComponent<Transform>().localPosition;
		Vector3 rightHandPos =  rightHand.PalmPosition.ToUnityTranslated() / 100;
		rightHandPos.y -= 0.03f;
		rightHandPos.x -= 0.01f;
		GameObject.Find("RightHand").GetComponent<Transform>().localPosition = defaultPos + rightHandPos;
	}

	//Returns distance from hand hard deck
	public float[] checkHandHeight(Hand leftHand, Hand rightHand){
		float leftHandHeight = leftHand.PalmPosition.y;
		float rightHandHeight = rightHand.PalmPosition.y;
		
		float[] distances = new float[2];
		distances[0] = leftHandHeight - HAND_HEIGHT_LEAP;
		distances[1] = rightHandHeight - HAND_HEIGHT_LEAP;
		return distances;
	}
		
	private float average (float[] array)
	{
		float total = 0;		
		foreach (float item in array) {
			total += item;
		}
		float avg = total / array.Length;
		return avg;
	}
}
