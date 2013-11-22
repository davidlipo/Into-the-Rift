using UnityEngine;
using System.Collections;
using Leap;

public class Scanner : MonoBehaviour {
	
	int state;
	float startTime, journeyLength;
	Vector3 start, end, initialPosition;
	private Controller controller;
	private Frame latestFrame;
	
	const float LIGHT_INTENSITY = 0.3f;
	const float SCANNING_DIST = 0.5f;
	const float SCAN_SPEED = 0.2f;
	
	// Use this for initialization
	void Start () {
		try{
			controller = new Controller();
			hideHologram();
			state = 0;
		}catch{
			Application.Quit();
		}	
	}
	
	// Update is called once per frame
	void Update () {
		latestFrame = controller.Frame();
		switch (state) {
		//Nothing
		case 0:
			if (latestFrame.Hands.Count == 2) {
				state = 1;
				setScanningVars();
				turnLightsOn();
			}
			
			break;
		//Scanning up
		case 1:
			scanningSequenceUp();
			if (transform.position.y >= end.y) {
				state = 2;
				startTime = Time.time;
			}
			
			break;
		//Scanning down
		case 2:
			scanningSequenceDown();
			
			if (transform.position.y <= start.y) {
				state = 3;
			}
			
			break;
		//Done!
		case 3:
			turnLightsOff();
			showHologram();
			break;
		
	}
}
	
	void setScanningVars() {
		start = transform.position;
		end = new Vector3 (transform.position.x, transform.position.y + SCANNING_DIST, transform.position.z);
		startTime = Time.time;
		journeyLength = Vector3.Distance(start, end);
	}
	
	void scanningSequenceUp(){
		//Distance moved = time * speed.
		float distCovered = (Time.time - startTime) * SCAN_SPEED;
		// Fraction of journey completed = current distance divided by total distance.
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp (start, end, fracJourney);
	}
	
	void scanningSequenceDown(){
		//Distance moved = time * speed.
		float distCovered = (Time.time - startTime) * SCAN_SPEED;
		// Fraction of journey completed = current distance divided by total distance.
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp (end, start, fracJourney);
	}
	
	private void turnLightsOn(){
		GameObject.Find("leftLight1").GetComponent<Light>().intensity = LIGHT_INTENSITY;
		GameObject.Find("leftLight2").GetComponent<Light>().intensity = LIGHT_INTENSITY;
		GameObject.Find("rightLight1").GetComponent<Light>().intensity = LIGHT_INTENSITY;
		GameObject.Find("rightLight2").GetComponent<Light>().intensity = LIGHT_INTENSITY;
	}
	
	private void turnLightsOff(){
		GameObject.Find("leftLight1").GetComponent<Light>().intensity = 0.0f;
		GameObject.Find("leftLight2").GetComponent<Light>().intensity = 0.0f;
		GameObject.Find("rightLight1").GetComponent<Light>().intensity = 0.0f;
		GameObject.Find("rightLight2").GetComponent<Light>().intensity = 0.0f;
	}
	
	void hideHologram(){
		GameObject.Find("BodyHol").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("EngineHolLeft").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("EngineHolRight").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("TailHol").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("WingsHol").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("HoloLight").GetComponent<Light>().intensity = 0f;
		//GameObject.Find("Interaction Area").GetComponent<MeshRenderer>().enabled = false;
	}
	
	void showHologram(){
		GameObject.Find("BodyHol").GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find("EngineHolLeft").GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find("EngineHolRight").GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find("TailHol").GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find("WingsHol").GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find("HoloLight").GetComponent<Light>().intensity = 0.3f;
		//GameObject.Find("Interaction Area").GetComponent<MeshRenderer>().enabled = true;
	}
	
}
