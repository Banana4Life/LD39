using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject mainBase;
	public List<GameObject> entities = new List<GameObject>();
	public GameObject type1;

	public float timeToSpawn;
	public float interval = 3;
	private int enemyCount;

	public int enemyPerWave = 5;
	private int enemiesToSpawn;

	// Use this for initialization
	void Start ()
	{
		enemiesToSpawn = enemyPerWave;
		timeToSpawn = interval;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timeToSpawn -= Time.deltaTime;
		if (timeToSpawn <= 0)
		{
			timeToSpawn = interval;
			SpawnEnemy(type1);
		}
	}

	public void SpawnEnemy(GameObject type)
	{
		if (enemiesToSpawn > 0)
		{
			enemiesToSpawn--;
			var enemy = Instantiate(type, gameObject.transform);
			enemy.name = "Enemy " + enemyCount++;
			var enemyController = enemy.GetComponent<EnemyController>();
			enemyController.mineObject = gameObject;
			enemyController.baseObject = mainBase;
		}
	}
}
