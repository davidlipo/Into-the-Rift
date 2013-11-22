using UnityEngine;
using System.Collections;

public class AsteroidGeneratorBoxScript : MonoBehaviour {
	
	public float xBounds;
	public float yBounds;
	public float zBounds;
	public int asteroidLimit;
	private int currentAsteroidNum;
	private Vector3 prevPosition;
	
	// Use this for initialization
	public void decreaseAsteroidNum() {
		currentAsteroidNum -= 1;
	}
	
	void Start () {
		currentAsteroidNum = 0;
		InitialiseAsteroidField(asteroidLimit);
	}
	
	// Update is called once per frame
	
	void InitialiseAsteroidField(int num) {
		for (int i = 0; i < num; i++)
			SpawnAsteroid (GenerateInitialPoint());
	}
	
	Vector3 GenerateInitialPoint() {
		float randomX = Random.Range(transform.position.x - xBounds, transform.position.x + xBounds);
		float randomY = Random.Range(transform.position.y - yBounds, transform.position.y + yBounds);
		float randomZ = Random.Range(transform.position.z - zBounds, transform.position.z + zBounds);
		return new Vector3(randomX, randomY, randomZ);
	}
	
	void SpawnAsteroid(Vector3 pos) {
		currentAsteroidNum++;
		GameObject asteroidClone;
		asteroidClone = Instantiate(Resources.Load("Asteroids/AsteroidLowPolyPrefab"), pos, Quaternion.identity) as GameObject;
		asteroidClone.GetComponent<AsteroidScript>().SetGenerator(this.gameObject);
	}
}
