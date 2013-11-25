using UnityEngine;
using System.Collections;
using Leap;
/*
 * Space to turn lights on
 * Right shift to lift ship
 * Left shift to launch ship
 */

// TODO: Acceleration and smooth movement


public class Lauch : MonoBehaviour
{
	
	public int liftDelay;
	public float liftForce;
	private Vector3 start, end;
	private Vector3 origin, gateTrigger;
	private int counter;
	
	private Vector3 topGateOpenStart, topGateOpenEnd, bottomGateOpenStart, bottomGateOpenEnd;
	private Vector3 topGateCloseStart, topGateCloseEnd, bottomGateCloseStart, bottomGateCloseEnd;
	
	float startTime, journeyLength;
	float topGateOpenStartTime, topGateOpenEndTime, bottomGateOpenStartTime, bottomGateOpenEndTime;
	float topGateOpenJourneyLength, bottomGateOpenJourneyLength;
	
	float topGateCloseStartTime, topGateCloseEndTime, bottomGateCloseStartTime, bottomGateCloseEndTime;
	float topGateCloseJourneyLength, bottomGateCloseJourneyLength;
	
	public AudioClip jetStartup;
	public float volume;
	private bool jetPlayed = false;
	
	private Controller controller;
	State state;
	
	const float LIFT_SPEED = 1.0f;
	const float LAUNCH_SPEED = 20.0f;
	const float LIFT_DIST = 0.5f;
	const float LAUNCH_DIST = 70;
	const float GATE_OPEN_SPEED = 5.0f;
	const float GATE_OPEN_DIST = 3.5f;
	
	string info = "";
	
	//string info;
	
	// Use this for initialization
	void Start () {
		try{
			controller = new Controller();
			state = State.NO_INPUT;
			origin = transform.position;
			gateTrigger = new Vector3(origin.x, origin.y + LIFT_DIST, GameObject.Find ("GateTop").transform.position.z - 4); //just before gate
		}catch{
			Application.Quit();
		}		
	}
	
	void OnGUI(){
		GUI.Box (new Rect (0, 0, UnityEngine.Screen.width, UnityEngine.Screen.height), info);
	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		Frame latestFrame = controller.Frame();
		SkipLaunchSequence();
		//Debug.Log ("State: " + state.ToString());
		switch (state) {
		case State.NO_INPUT:
			onNoInput(latestFrame.Hands.Count);
			break;
		case State.CHECK_ACCELERATION:
			checkAcceleration(latestFrame);
			break;
		case State.CHECK_DECELERATION:
			checkDeceleration(latestFrame);
			break;
		case State.CHECK_BANK_LEFT:
			checkBankLeft(latestFrame);
			break;
		case State.CHECK_BANK_RIGHT:	//Check Bank Right
			Vector3 handDiff2 = latestFrame.Hands.Leftmost.PalmPosition.ToUnityScaled() - latestFrame.Hands.Rightmost.PalmPosition.ToUnityScaled();
			info = "Bank Right, Move Right Hand Down \n Hand Diff: " + handDiff2;
			if(handDiff2.y > 2){
				state = State.CHECK_YAW_LEFT;
			}
			break;
		case State.CHECK_YAW_LEFT:	//Check Yaw Left
			Vector3 handDiff3 = latestFrame.Hands.Leftmost.PalmPosition.ToUnityScaled() - latestFrame.Hands.Rightmost.PalmPosition.ToUnityScaled();
			info = "Yaw Left, Move Left Hand Back \n Hand Diff: " + handDiff3;
			if(handDiff3.z < -1.5){
				state = State.CHECK_YAW_RIGHT;
			}
			break;
		case State.CHECK_YAW_RIGHT:	//Check Yaw Right
			Vector3 handDiff4 = latestFrame.Hands.Leftmost.PalmPosition.ToUnityScaled() - latestFrame.Hands.Rightmost.PalmPosition.ToUnityScaled();
			info = "Yaw Right, Move Right Hand Back \n Hand Diff: " + handDiff4;
			if(handDiff4.z > 1.5){
				state = State.LIFTING;
			}
			break;
		case State.LIFTING:
			state = State.WAITING_FOR_LAUNCH;
			if(!jetPlayed){
				GetComponent<AudioSource>().PlayOneShot(jetStartup, volume);
				jetPlayed = true;
			}
			setLiftVars();
			break;
		case State.WAITING_FOR_LAUNCH:
			liftShip();
			float averageZ3 = (latestFrame.Hands.Leftmost.PalmPosition.z + latestFrame.Hands.Rightmost.PalmPosition.z) / 2;
			info = "Launch, Move Hands Forward \n Average Z: " + averageZ3;
			
			if(averageZ3 < -30){
				state = State.OPEN_GATE;
				setLaunchVars();
			}
			break;
		case State.OPEN_GATE:
			launchShip();
			//Debug.Log("Ship: " + transform.position.z + " Gate: " + gateTrigger.z);
			if(transform.position.z > gateTrigger.z){
				//GameObject.Find("AsteroidGenerator").GetComponent<AsteroidGeneratorBoxScript>().enabled = true;
				setOpenGateVars();
				state = State.CLOSE_GATE;
			}
			break;
		case State.CLOSE_GATE:
			launchShip();
			openGate();
			if(transform.position.z > GameObject.Find ("GateTop").transform.position.z){
				//Debug.Log ("Out of Hangar");
				GameObject.Find("Star").GetComponent<Light>().intensity = 1.2f;
				setCloseGateVars();
				state = State.LAUNCHING;
			}
			break;
		//Closing gate when past
		case State.LAUNCHING:
			launchShip();
			closeGate();
			state = State.FINISHED;
			break;
		case State.FINISHED:
			closeGate();
			GetComponent<AudioSource>().volume = 0;
			GetComponent<LeapControl>().launched = true;
			GetComponent<Stabiliser>().enabled = true;
			this.GetComponent<Lauch>().enabled = false;
			break;
		}
		
	}

	private void SkipLaunchSequence() {
		if (Input.GetKeyDown(KeyCode.Space))
			state++;
	}
	
	private void onNoInput(int handCount){
		info = "Place Both Hands Over the Leap";
		if(handCount== 2){
			state = State.CHECK_ACCELERATION;
		}
	}
	
	private Float getAverageZ(Frame latestFrame){
		if(latestFrame.Hands.Count == 2){
			float leftHandZ = latestFrame.Hands.Leftmost.PalmPosition.z;
			float rightHandZ = latestFrame.Hands.Rightmost.PalmPosition.z;
			return new Float((leftHandZ + rightHandZ) / 2);	
		}else{
			info = "Please Place Both Hands Over the Leap";
			return null;
		}
	}
	
	private void checkAcceleration(Frame latestFrame){
		Float averageZ = getAverageZ(latestFrame);
		if(averageZ != null){
			info = "Acceleration, Move Hands Forward \n Average Z: " + averageZ;
			if(averageZ.value < -30){
				state = State.CHECK_DECELERATION;
			}
		}
	}
	
	private void checkDeceleration(Frame latestFrame){
		Float averageZ = getAverageZ(latestFrame);
		if(averageZ != null){
			info = "Deceleration, Move Hands Backwards \n Average Z: " + averageZ;
			if(averageZ.value > 30){
				state = State.CHECK_BANK_LEFT;
			}
		}
	}
	
	private void checkBankLeft(Frame latestFrame){
		Vector3 handDiff = latestFrame.Hands.Leftmost.PalmPosition.ToUnityScaled() - latestFrame.Hands.Rightmost.PalmPosition.ToUnityScaled();	
		info = "Bank Left, Move Left Hand Down \n Hand Diff: " + handDiff;
		if(handDiff.y < -2){
			state = State.CHECK_BANK_RIGHT;
		}
	}
	
	void setLiftVars() {
		//start = transform.position;
		//end = new Vector3 (transform.position.x, transform.position.y + LIFT_DIST, transform.position.z);
		//startTime = Time.time;
		//journeyLength = Vector3.Distance(start, end);
	}
	
	void liftShip() {
		for(int i = 0; i < liftDelay; i++){
			transform.rigidbody.AddForce(transform.up * liftForce);
		}
		transform.rigidbody.velocity = new Vector3(0,0,0);
		//Distance moved = time * speed.
		//float distCovered = (Time.time - startTime) * LIFT_SPEED;
		// Fraction of journey completed = current distance divided by total distance.
		//float fracJourney = distCovered / journeyLength;
		//transform.position = Vector3.Lerp (start, end, fracJourney);
		
	}
	
	void setLaunchVars() {
		start = transform.position;
		end = new Vector3 (transform.position.x, transform.position.y, transform.position.z + LAUNCH_DIST);
		startTime = Time.time;
		journeyLength = Vector3.Distance(start, end);
	}
	
	void launchShip() {	
		// Distance moved = time * speed.
		float distCovered = (Time.time - startTime) * LAUNCH_SPEED;
		// Fraction of journey completed = current distance divided by total distance.
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp (start, end, fracJourney);
	}
	
	void setOpenGateVars() {
		topGateOpenStart = GameObject.Find ("GateTop").transform.position;
		
		topGateOpenEnd = new Vector3 ( GameObject.Find ("GateTop").transform.position.x, 
			GameObject.Find ("GateTop").transform.position.y + GATE_OPEN_DIST, 
			GameObject.Find ("GateTop").transform.position.z);
		
		topGateOpenStartTime = Time.time;
		topGateOpenJourneyLength = Vector3.Distance(topGateOpenStart, topGateOpenEnd);
		
		bottomGateOpenStart = GameObject.Find ("GateBottom").transform.position;
		
		bottomGateOpenEnd = new Vector3 (GameObject.Find ("GateBottom").transform.position.x, 
			GameObject.Find ("GateBottom").transform.position.y - GATE_OPEN_DIST, 
			GameObject.Find ("GateBottom").transform.position.z);
		
		bottomGateOpenStartTime = Time.time;
		bottomGateOpenJourneyLength = Vector3.Distance(bottomGateOpenStart, bottomGateOpenEnd);
	}
	
	void setCloseGateVars() {
		topGateCloseStart = GameObject.Find ("GateTop").transform.position;
		
		topGateCloseEnd = new Vector3 ( GameObject.Find ("GateTop").transform.position.x, 
			GameObject.Find ("GateTop").transform.position.y - GATE_OPEN_DIST, 
			GameObject.Find ("GateTop").transform.position.z);
		
		topGateCloseStartTime = Time.time;
		topGateCloseJourneyLength = Vector3.Distance(topGateCloseStart, topGateCloseEnd);
		
		bottomGateCloseStart = GameObject.Find ("GateBottom").transform.position;
		
		bottomGateCloseEnd = new Vector3 (GameObject.Find ("GateBottom").transform.position.x, 
			GameObject.Find ("GateBottom").transform.position.y + GATE_OPEN_DIST, 
			GameObject.Find ("GateBottom").transform.position.z);
		
		bottomGateCloseStartTime = Time.time;
		bottomGateCloseJourneyLength = Vector3.Distance(bottomGateCloseStart, bottomGateCloseEnd);
	}
	
	void openGate() {
		// Distance moved = time * speed.
		float topGateDistCovered = (Time.time - topGateOpenStartTime) * GATE_OPEN_SPEED;
		// Fraction of journey completed = current distance divided by total distance.
		float topGateFracJourney = topGateDistCovered / topGateOpenJourneyLength;
		//GameObject.Find("GateTop").transform.Translate(new Vector3(10,10,10));
		GameObject.Find ("GateTop").transform.position = Vector3.Lerp (topGateOpenStart, topGateOpenEnd, topGateFracJourney);
		
		// Distance moved = time * speed.
		float bottomGateDistCovered = (Time.time - bottomGateOpenStartTime) * GATE_OPEN_SPEED;
		// Fraction of journey completed = current distance divided by total distance.
		float bottomGateFracJourney = bottomGateDistCovered / bottomGateOpenJourneyLength;
		GameObject.Find ("GateBottom").transform.position = Vector3.Lerp (bottomGateOpenStart, bottomGateOpenEnd, bottomGateFracJourney);
	}
	
	void closeGate() {
		// Distance moved = time * speed.
		float topGateDistCovered = (Time.time - topGateCloseStartTime) * GATE_OPEN_SPEED;
		// Fraction of journey completed = current distance divided by total distance.
		float topGateFracJourney = topGateDistCovered / topGateCloseJourneyLength;
		//GameObject.Find("GateTop").transform.Translate(new Vector3(10,10,10));
		GameObject.Find ("GateTop").transform.position = Vector3.Lerp (topGateCloseStart, topGateCloseEnd, topGateFracJourney);
		
		
		// Distance moved = time * speed.
		float bottomGateDistCovered = (Time.time - bottomGateCloseStartTime) * GATE_OPEN_SPEED;
		// Fraction of journey completed = current distance divided by total distance.
		float bottomGateFracJourney = bottomGateDistCovered / bottomGateCloseJourneyLength;
		GameObject.Find ("GateBottom").transform.position = Vector3.Lerp (bottomGateCloseStart, bottomGateCloseEnd, bottomGateFracJourney);
	}
	
	private class Float{
		public float value;
		public Float(float value) {this.value = value;}
	}

	private enum State{
		NO_INPUT,
		CHECK_ACCELERATION,
		CHECK_DECELERATION,
		CHECK_BANK_LEFT,
		CHECK_BANK_RIGHT,
		CHECK_YAW_LEFT,
		CHECK_YAW_RIGHT,
		LIFTING,
		WAITING_FOR_LAUNCH,
		LAUNCHING,
		OPEN_GATE,
		CLOSE_GATE,
		FINISHED
	}
}
