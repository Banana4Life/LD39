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
	public override float GetPropertyHeight(SerializedProperty property,
		GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}
 
	public override void OnGUI(Rect position,
		SerializedProperty property,
		GUIContent label)
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

	[ReadOnly] public int nextType;
	[ReadOnly] public float timeToSpawn;
	public float interval = 3;
	[ReadOnly] public int enemyCount = 1;

	public int enemyPerWave = 5;
	[ReadOnly] public int enemiesToSpawn;

	public bool dead;

	// Use this for initialization
	void Start ()
	{
		nextType = 0;
		enemiesToSpawn = enemyPerWave;
		timeToSpawn = interval;
		dead = false;
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
		if (enemiesToSpawn > 0)
		{
			enemiesToSpawn--;
			var enemy = Instantiate(type, gameObject.transform);
			enemy.name = "Enemy LVL " + (nextType + 1) + ": " + enemyCount++;
			var enemyController = enemy.GetComponent<EnemyController>();
			enemyController.mineObject = gameObject;
			enemyController.baseObject = mainBase;
			enemyController.transform.position = gameObject.transform.position;
		}
		if (gameObject.transform.childCount == 0)
		{
			Debug.Log("Empty Lane! Upgrading Defences...");
			dead = true;
			var spawners = gameObject.transform.parent.GetComponentsInChildren<Spawner>();
			foreach (var spawner in spawners)
			{
				spawner.UpgradeDefences();
			}
		}
	}

	public void UpgradeDefences()
	{
		if (types.Count > nextType)
		{
			nextType++;
		}
		enemiesToSpawn = enemyPerWave;
	}
}
