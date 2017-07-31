using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {

    public float interval = 0.5f;
    private float timeToSpawn;

    public GameObject cloudCleaner;

	public bool active;

	void Update()
	{
		if (active)
		{
			timeToSpawn -= Time.deltaTime;
			if (timeToSpawn < 0)
			{
				Instantiate(cloudCleaner, gameObject.transform.position, Quaternion.identity, GameObject.Find("Exhaust").transform);
				timeToSpawn = interval;
			}	
		}
		
	}

}
