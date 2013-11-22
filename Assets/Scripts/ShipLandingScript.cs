using UnityEngine;
using System.Collections;

public class ShipLandingScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space))
		{
			transform.position += new Vector3(-0.7f, -0.1f, 0);
		}
	}
}
