using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {
	
	public float maxSize;
	public float minSize;
	public float maxForce;
	public float minForce;
	public float fadeRate;
	private float alpha;
	private GameObject generator;
	private float size;
	private float force;
	private float bounds;
	private bool outOfBounds;
	private bool fadeIn;
	
	// Use this for initialization
	
	public void SetGenerator(GameObject gen)
	{
		generator = gen;
		bounds = generator.GetComponent<AsteroidGeneratorBoxScript>().Size;
	}
	
	void Start () {
		alpha = 1;
		fadeIn = false;
		outOfBounds = false;
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
		if (!outOfBounds) { //Within Bounds
			if (generator != null && GameObject.Find("ShipModel").GetComponent<DistanceToMoonScript>().getOutsideDespawn()) {		
				if (Vector3.Distance(transform.position, generator.transform.position) > bounds) {
					outOfBounds = true;
				}
			}
		} else { //Out of Bounds
			if  (!fadeIn) {  //Fading Out
				if (alpha > 0) {
					alpha -= fadeRate;
					renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);
				} else { //Object is Transparent
					fadeIn = true;
					alpha = 0;
					transform.position = Random.onUnitSphere * (0.98f * bounds) + generator.transform.position;
					Reset ();
				}
			} else { //Fading In
				if (alpha < 1) {
					alpha += fadeRate;
					renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);
				} else { //Object is Visible
					alpha = 1;
					fadeIn = false;
					outOfBounds = false;
				}
			}
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
