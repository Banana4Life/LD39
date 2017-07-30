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
	public List<GameObject> types;

	private Queue<GameObject> entities = new Queue<GameObject>();

	[ReadOnly] public int nextType = -1;
	[ReadOnly] public float timeToSpawn;
	public float interval = 3;
	[ReadOnly] public int enemyCount = 1;

	public int enemyPerWave = 5;

	public bool dead;

	// Use this for initialization
	void Start ()
	{
		nextType = 0;
		timeToSpawn = interval;
		dead = false;
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
		var spawners = gameObject.transform.parent.GetComponentsInChildren<Spawner>();

		if (entities.Count > 0)
		{
			entities.Dequeue().SetActive(true);
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

	private GameObject doSpawnEnemy(GameObject type)
	{
		var enemy = Instantiate(type, gameObject.transform.position, Quaternion.identity, gameObject.transform);
		enemy.name = "Enemy LVL " + (nextType + 1) + ": " + enemyCount++;
		var enemyController = enemy.GetComponent<EnemyController>();
		enemyController.mineObject = gameObject;
		enemyController.baseObject = mainBase;
		enemyController.active = false;
		return enemy;
	}

	public void UpgradeDefences()
	{
		if (types.Count > nextType)
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
			var spawned = doSpawnEnemy(types[0]);
			
			entities.Enqueue(spawned);
			spawned.SetActive(false);
		}
	}
}
