using UnityEngine;
using System.Collections;

public class FadeInScript : MonoBehaviour {
	
	public Texture white;
	public float fadeRate;
	float alpha;

	
	// Use this for initialization
	void Start () {
		alpha = 1;
	}
	
	void Update () {
		if (alpha > 0) {
			alpha -= fadeRate;
		} else {
			alpha = 0;
			Destroy (this);
		}
	}
	
	void OnGUI() {
		GUI.color = new Color(255, 255, 255, alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), white);
	}
}
