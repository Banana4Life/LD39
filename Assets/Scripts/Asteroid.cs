using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Destroyable
{

	void Start () 
	{
		// rotate randomly
		transform.rotation = Quaternion.Euler(0f, Random.value * 90f, 0f);
	}


}