using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Asteroid : Destroyable
{

	void Start () 
	{
		// rotate randomly
		transform.rotation = Quaternion.Euler(0f, Random.value * 90f, 0f);
		var pos = transform.position;
		pos.y += Random.Range(-0.05f, 0.05f);
		transform.position = pos;
	}


}