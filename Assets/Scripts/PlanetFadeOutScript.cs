using UnityEngine;
using System.Collections;

public class PlanetFadeOutScript : MonoBehaviour {
	
	public Texture white;
	public float fadeRate;
	float alpha;
	bool beginFade;
	
	// Use this for initialization
	void Start () {
		alpha = 0;
		beginFade = false;
	}
	
	void OnTriggerEnter () {
		Debug.Log ("Trigger");
		GameObject.Find("ShipModel").GetComponent<BoxCollider>().enabled = false;
		beginFade = true;
		//Camera.main.GetComponent<CameraShake>().Intensity = 0.001f;
	}
	
	void Update () {
		
		if (beginFade) {
			Debug.Log ("Alpha: " + alpha);
			if (alpha < 1) {
				alpha += fadeRate;
			} else {
				alpha = 1;
				Application.LoadLevel("PlanetLandingTest");
			}
		}
	}
	
	void OnGUI() {
		GUI.color = new Color(219, 162, 58, alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), white);
	}
}
