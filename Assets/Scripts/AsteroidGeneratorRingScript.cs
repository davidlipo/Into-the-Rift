using UnityEngine;
using System.Collections;

public class AsteroidGeneratorRingScript : MonoBehaviour {

	public float Radius;
	public float Offset;
	public int asteroidNum;
	public float rotateAngle;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
			SpawnAsteroid(asteroidNum);
	}
	
	void SpawnAsteroid(int num) {
		for (int i = 0; i < num; i++) {
			float randomAngle = Mathf.Deg2Rad * (Random.Range(0, 360) * Mathf.PI*2);
			float randomX = Mathf.Cos(randomAngle) * Radius;
			Debug.Log(randomX);
			float randomY = Mathf.Sin(randomAngle) * Radius;
			Debug.Log (randomY);
			Vector3 randomOffset = new Vector3(Random.Range (-Offset, Offset), Random.Range (-Offset, Offset), Random.Range (-Offset, Offset));
			Vector3 newPos = new Vector3(randomX, randomY, 0) + randomOffset + gameObject.transform.position;
			GameObject asteroidClone;
			switch (Random.Range (1, 5)) {
				case 1:
					asteroidClone = Instantiate(Resources.LoadAssetAtPath("Assets/Prefabs/AsteroidPrefab1.prefab", typeof(GameObject)), newPos, Quaternion.identity) as GameObject;
					asteroidClone.transform.RotateAround(transform.position, Vector3.left, rotateAngle);	
					break;
				
				case 2:
					asteroidClone = Instantiate(Resources.LoadAssetAtPath("Assets/Prefabs/AsteroidPrefab2.prefab", typeof(GameObject)), newPos, Quaternion.identity) as GameObject;
					asteroidClone.transform.RotateAround(transform.position, Vector3.left, rotateAngle);	
					break;
				
				case 3:
					asteroidClone = Instantiate(Resources.LoadAssetAtPath("Assets/Prefabs/AsteroidPrefab3.prefab", typeof(GameObject)), newPos, Quaternion.identity) as GameObject;
					asteroidClone.transform.RotateAround(transform.position, Vector3.left, rotateAngle);	
					break;
				
				case 4:
					asteroidClone = Instantiate(Resources.LoadAssetAtPath("Assets/Prefabs/AsteroidPrefab4.prefab", typeof(GameObject)), newPos, Quaternion.identity) as GameObject;
					asteroidClone.transform.RotateAround(transform.position, Vector3.left, rotateAngle);	
					break;
				
				case 5:
					asteroidClone = Instantiate(Resources.LoadAssetAtPath("Assets/Prefabs/AsteroidPrefab5.prefab", typeof(GameObject)), newPos, Quaternion.identity) as GameObject;
					asteroidClone.transform.RotateAround(transform.position, Vector3.left, rotateAngle);	
					break;
			}
		}
	}
}
