using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCleaner : MonoBehaviour
{
	public float destroyIn = 5f;
	public float countdown;
	// Use this for initialization
	void Start ()
	{
		countdown = destroyIn;
	}
	
	// Update is called once per frame
	void Update ()
	{
		countdown -= Time.deltaTime;
		if (countdown < 0)
		{
			Destroy(gameObject);
		}
	}
}
