using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	
	public float angularVelocity;
	public GameObject spinner1, spinner2, spinner3;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		spinner1.transform.rigidbody.angularVelocity = new Vector3(0, angularVelocity, 0);
		spinner2.transform.rigidbody.angularVelocity = new Vector3(0, angularVelocity, 0);
		spinner3.transform.rigidbody.angularVelocity = new Vector3(0, angularVelocity, 0);
	}
}
