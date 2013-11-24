using UnityEngine;
using System.Collections;

public class NoLeapControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space))
			transform.position += transform.forward * 50;
	}
}
