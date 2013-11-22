using UnityEngine;
using System.Collections;

public class FadeOutScript : MonoBehaviour {
	
	public float fadeRate;
	public Texture white;
	float alpha;
	bool beginFade;
	
	public void BeginFade() {
		beginFade = true;
	}
	// Use this for initialization
	void Start () {
		beginFade = false;
		alpha = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (beginFade) {
			if (alpha < 1) {
				alpha += fadeRate;
			} else {
				alpha = 1;
				Application.LoadLevel("PlanetLandingTest");
			}
		}
	}
	
	void OnGUI() {
		GUI.color = new Color(255, 255, 255, alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), white);
	}
}
