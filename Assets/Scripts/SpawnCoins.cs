using UnityEngine;
using System.Collections;

public class SpawnCoins : MonoBehaviour {

	public GameObject[] coinPrefabs;

	public float minSecondsBetweenSpawning = 3.0f;
	public float maxSecondsBetweenSpawning = 6.0f;

	
	private float savedTime;
	private float secondsBetweenSpawning;

	// Use this for initialization
	void Start () {
		savedTime = Time.time;
		secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - savedTime >= secondsBetweenSpawning) // is it time to spawn again?
		{
			MakeThingToSpawn();
			savedTime = Time.time; // store for next spawn
			secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
		}	
	}

	void MakeThingToSpawn()
	{
		// create a new gameObject
		int len = coinPrefabs.GetLength(0);
		GameObject spawnPrefab = coinPrefabs[Random.Range(0, len)];
		Instantiate(spawnPrefab, transform.position, transform.rotation);
	}
}
