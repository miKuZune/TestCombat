using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicSpawner : MonoBehaviour {


	public GameObject objToSpawn;
	public float spawnTime;
	float timeSinceLastSpawn;
	// Use this for initialization
	void Start () {
		if (spawnTime == 0) {
			spawnTime = 5;
		}
	}

	void Spawn()
	{
		Instantiate (objToSpawn, transform.position, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () 
	{
		timeSinceLastSpawn += Time.deltaTime;

		if (timeSinceLastSpawn >= spawnTime) {
			Spawn ();
			timeSinceLastSpawn = 0;
		}
	}
}
