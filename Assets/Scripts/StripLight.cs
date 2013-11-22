using UnityEngine;
using System.Collections;
using Leap;

public class StripLight : MonoBehaviour {
	
	//**********************************************
	//MAKE SURE YOU HAVE YOUR LIGHTS NAMED "strip_1", "strip_2" and "strip_3" INSIDE OF UNITY
	//**********************************************
	
	public float LIGHT_INTENSITY;
	public int stripInterval;
	public int state;
	private int currentTime;
	private bool turnOnLights;
	private Controller controller;
	private Frame latestFrame;
	public AudioClip donk;
	public float volume;
	private bool donk1Played, donk2Played, donk3Played, donk4Played, donk5Played, donk6Played;
	
	void Start () {
		try{
			controller = new Controller();
			currentTime = 0;
			turnOnLights = false;
			donk1Played = donk2Played = donk3Played = donk4Played = donk5Played = donk6Played = false;
		}catch{
			Application.Quit();
		}
	
	}
	
	void FixedUpdate () {
		latestFrame = controller.Frame(0);
		
		switch(state){
		case 0:	//Waiting for input
			break;
		case 1:
			break;
		case 2:
			break;
		}
		
		if(latestFrame.Hands.Count == 2){
			turnOnLights = true;
		}
		
		if (turnOnLights)
			LightSequence();
	}
	
	void LightSequence() {
		currentTime++;
		if (currentTime > stripInterval){
			GameObject.Find ("spot_6").GetComponent<Light>().intensity = LIGHT_INTENSITY;
			if(!donk6Played){
				GameObject.Find ("spot_6").GetComponent<AudioSource>().PlayOneShot(donk, volume);
				donk6Played = true;
			}
		}
		if (currentTime > stripInterval * 2){
			GameObject.Find ("spot_5").GetComponent<Light>().intensity = LIGHT_INTENSITY;
			if(!donk5Played){
				GameObject.Find ("spot_5").GetComponent<AudioSource>().PlayOneShot(donk, volume);
				donk5Played = true;
			}
		}
		if (currentTime > stripInterval * 3){
			GameObject.Find ("spot_4").GetComponent<Light>().intensity = LIGHT_INTENSITY;
			if(!donk4Played){
				GameObject.Find ("spot_4").GetComponent<AudioSource>().PlayOneShot(donk, volume);
				donk4Played = true;
			}
		}
		if (currentTime > stripInterval * 4){
			GameObject.Find ("spot_3").GetComponent<Light>().intensity = LIGHT_INTENSITY;
			if(!donk3Played){
				GameObject.Find ("spot_3").GetComponent<AudioSource>().PlayOneShot(donk, volume);
				donk3Played = true;
			}
		}
		if (currentTime > stripInterval * 5){
			GameObject.Find ("spot_2").GetComponent<Light>().intensity = LIGHT_INTENSITY;
			if(!donk2Played){
				GameObject.Find ("spot_2").GetComponent<AudioSource>().PlayOneShot(donk, volume);
				donk2Played = true;
			}
		}
		if (currentTime > stripInterval * 6){
			GameObject.Find ("spot_1").GetComponent<Light>().intensity = LIGHT_INTENSITY;
			if(!donk1Played){
				GameObject.Find ("spot_1").GetComponent<AudioSource>().PlayOneShot(donk, volume);
				donk1Played = true;
			}
		}
	}
}

