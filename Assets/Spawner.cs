using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute
{
 
}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}
 
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true;
	}
}

public class Spawner : MonoBehaviour
{
	public GameObject mainBase;
	public GameObject miningBase;
	public List<GameObject> types;

	[ReadOnly] public Queue<GameObject> entities = new Queue<GameObject>();

	[ReadOnly] public int nextType;
	[ReadOnly] public float timeToSpawn;
	public float interval = 3;
	[ReadOnly] public int enemyCount;

	public int enemyPerWave = 5;

	public bool dead;

	// Use this for initialization
	void Start ()
	{
		nextType = -1;
		timeToSpawn = interval;
		dead = false;
		enemyCount = 0;
		UpgradeDefences();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!dead)
		{
			timeToSpawn -= Time.deltaTime;
			if (timeToSpawn <= 0)
			{
				timeToSpawn = interval;
				SpawnEnemy(types[nextType]);
			}	
		}
	}

	public void SpawnEnemy(GameObject type)
	{
		var spawners = gameObject.GetComponentsInChildren<Spawner>();

		if (entities.Count > 0)
		{
			var next = entities.Dequeue();
			if (next != null)
			{
				next.GetComponent<EnemyController>().active = true;
				next.GetComponent<Rigidbody>().isKinematic = false;
			}
		}
		else if (gameObject.transform.childCount == 0)
		{
			Debug.Log("Empty Lane! Upgrading Defences...");
			dead = true;
			
			foreach (var spawner in spawners)
			{
				spawner.UpgradeDefences();
			}
		}
	}

	private GameObject doSpawnEnemy(GameObject type, GameObject at)
	{
		var spawnPos = gameObject.transform.position;
		spawnPos.y = 0;
		var enemy = Instantiate(type, spawnPos, Quaternion.identity, gameObject.transform);
		enemy.name = "Enemy LVL " + (nextType + 1) + ": " + enemyCount++;
		var enemyController = enemy.GetComponent<EnemyController>();
		enemyController.mineObject = gameObject;
		enemyController.baseObject = mainBase;
		enemyController.active = false;
		enemy.GetComponent<Rigidbody>().isKinematic = true;
		return enemy;
	}

	public void UpgradeDefences()
	{
		if (dead)
		{
			return;
		}
		if (types.Count >= nextType)
		{
			nextType++;
		}
		var oldCount = entities.Count;
		foreach (var entity in entities)
		{
			Destroy(entity);
		}
		entities.Clear();
		for (var j = 0; j < enemyPerWave + oldCount; j++)
		{
			var spawned = doSpawnEnemy(types[nextType], miningBase);
			
			entities.Enqueue(spawned);
		}
	}
}
