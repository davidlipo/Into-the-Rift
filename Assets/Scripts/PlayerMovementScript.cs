using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {
	
	public float minThrust;
	public float maxThrust;
	private float currentThrust;

	// Use this for initialization
	void Start () {
		currentThrust = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		MovementChecks ();
		Thrust();
	}
	
	void MovementChecks() {
		if (Input.GetKey(KeyCode.W))
			currentThrust += 0.05f;
		if (Input.GetKey(KeyCode.S))
			currentThrust -= 0.05f;
		Mathf.Clamp (currentThrust, minThrust, maxThrust);
		
		if (Input.GetKey(KeyCode.UpArrow))
			this.transform.Rotate(Vector3.up * 2);
		if (Input.GetKey(KeyCode.DownArrow))
			this.transform.Rotate(Vector3.down * 2);
		if (Input.GetKey(KeyCode.RightArrow))
			this.transform.Rotate(Vector3.back * 2);
		if (Input.GetKey(KeyCode.LeftArrow))
			this.transform.Rotate(Vector3.forward * 2);
	}
	
	void Thrust() {
		this.rigidbody.AddForce(transform.forward * currentThrust, ForceMode.Force); 
	}
}
