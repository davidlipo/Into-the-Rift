using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {
	
	public float maxSize;
	public float minSize;
	public float maxForce;
	public float minForce;
	private GameObject generator;
	private float size;
	private float force;
	private float xBounds;
	private float yBounds;
	private float zBounds;
	
	// Use this for initialization
	
	public void SetGenerator(GameObject gen)
	{
		generator = gen;
		xBounds = generator.GetComponent<AsteroidGeneratorBoxScript>().xBounds;
		yBounds = generator.GetComponent<AsteroidGeneratorBoxScript>().yBounds;
		zBounds = generator.GetComponent<AsteroidGeneratorBoxScript>().zBounds;
	}
	
	void Start () {
		size = Random.Range(minSize, maxSize);
		force = Random.Range(minForce, maxForce);
		Vector3 randomRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		transform.Rotate(Random.Range (0, 360), Random.Range (0, 360), Random.Range(0, 360));
		transform.localScale = new Vector3 (size, size, size);
		rigidbody.AddTorque(randomRotation * Random.Range (0, 10));
		rigidbody.AddForce(transform.forward * force);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (generator != null) {
			float xDiff = 0;
			float yDiff = 0;
			float zDiff = 0;
			
			if (transform.position.x < generator.transform.position.x - xBounds) {
				xDiff += xBounds * 1.9f;
				Reset();
			} else if (transform.position.x > generator.transform.position.x + xBounds) {
				xDiff -= xBounds * 1.9f;
				Reset();
			}
			
			if (transform.position.y < generator.transform.position.y - yBounds) {
				yDiff += yBounds * 1.9f;
				Reset();
			} else if (transform.position.y > generator.transform.position.y + yBounds) {
				yDiff -= yBounds * 1.9f;
				Reset();
			} 
			
			if (transform.position.z < generator.transform.position.z - zBounds) {
				zDiff += zBounds * 1.9f;
				Reset();
			} else if (transform.position.z > generator.transform.position.z + zBounds) {
				zDiff -= zBounds * 1.9f;
				Reset();
			}
			
			gameObject.transform.position += new Vector3(xDiff, yDiff, zDiff);
		}
	}
	
	void Reset() {
		size = Random.Range(minSize, maxSize);
		force = Random.Range(minForce, maxForce);
		gameObject.rigidbody.velocity = Vector3.zero;
		//Vector3 randomRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		//transform.Rotate(Random.Range (0, 360), Random.Range (0, 360), Random.Range(0, 360));
		//transform.localScale = new Vector3 (size, size, size);
		//rigidbody.AddTorque(randomRotation * Random.Range (300, 2500));
		//rigidbody.AddForce(transform.forward * force);
	}
}
