using UnityEngine;
using System.Collections;

public class AsteroidGeneratorBoxScript : MonoBehaviour {
	
	public float Size;
	public float InitialBarrier;
	public int AsteroidLimit;
	private int currentAsteroidNum;
	
	// Use this for initialization
	public void decreaseAsteroidNum() {
		currentAsteroidNum -= 1;
	}
	
	void Start () {
		currentAsteroidNum = 0;
		InitialiseAsteroidField(AsteroidLimit);
	}
	
	// Update is called once per frame
	
	void InitialiseAsteroidField(int num) {
		for (int i = 0; i < num; i++)
			SpawnAsteroid (GenerateInitialPoint());
	}
	
	Vector3 GenerateInitialPoint() {
		/*Vector3 randomVec = Vector3.zero;
		while (Vector3.Distance(Vector3.zero, randomVec) < InitialBarrier)
			randomVec = Random.insideUnitSphere * Size;
		return randomVec;*/
		return Random.onUnitSphere * Random.Range(InitialBarrier, Size);
	}
	
	void SpawnAsteroid(Vector3 pos) {
		currentAsteroidNum++;
		GameObject asteroidClone;
		asteroidClone = Instantiate(Resources.Load("Asteroids/AsteroidLowPolyPrefab"), pos, Quaternion.identity) as GameObject;
		asteroidClone.GetComponent<AsteroidScript>().SetGenerator(this.gameObject);
	}
}
