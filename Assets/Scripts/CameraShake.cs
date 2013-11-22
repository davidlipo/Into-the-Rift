using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	
	public float Intensity;
	public int ShakeRate;
	public Transform DefaultPos;
	int currentShakeFrame;
	
	// Use this for initialization
	void Start () {
		currentShakeFrame = 0;
	}
	
	// Update is called once per frame
	void Update () {
		currentShakeFrame++;
		if (currentShakeFrame >= ShakeRate) {
			Vector3 randomShakeVector = new Vector3(Random.Range(-Intensity, Intensity), Random.Range(-Intensity, Intensity), Random.Range(-Intensity, Intensity));
			gameObject.transform.position = DefaultPos.position + randomShakeVector;
			currentShakeFrame = 0;
		}
	}
}
