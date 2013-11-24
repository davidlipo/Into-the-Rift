using UnityEngine;
using System.Collections;

public class DistanceToMoonScript : MonoBehaviour {

	private bool outsideDespawnDist = true;
	private Vector3 moonPos;
	private float moonSize;
	private float spawnBounds;

	public bool getOutsideDespawn() {
		return outsideDespawnDist;
	}

	// Use this for initialization
	void Start () {
		moonPos = GameObject.Find ("Moon").transform.position;
		moonSize = GameObject.Find ("Moon").transform.localScale.x;
		spawnBounds = GameObject.Find ("AsteroidGenerator").GetComponent<AsteroidGeneratorBoxScript>().Size;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(GameObject.Find("ShipModel").transform.position, moonPos) > moonSize)
			outsideDespawnDist = true;
		else
			outsideDespawnDist = false;
	}
}
