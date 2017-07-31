using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {

    public float interval = 0.5f;
    [ReadOnly] public float timeToSpawn;

    public GameObject cloudCleaner;

	public bool activeEngine;

	public void Update()
	{
		timeToSpawn -= Time.deltaTime;
		if (cloudCleaner != null)
		{
			if (activeEngine)
			{
				if (timeToSpawn < 0)
				{
					Instantiate(cloudCleaner, gameObject.transform.position, Quaternion.identity, GameObject.Find("Exhaust").transform);
					timeToSpawn = interval;
				}	
			}	
		}
		
		doUpdate();

	}

	protected virtual void doUpdate()
	{
	}
}
