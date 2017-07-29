using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

	void Start () {
		// rotate randomly
		transform.rotation = Quaternion.Euler(90f, Random.value * 90f, 0f);
	}
	
	public void Explode()
	{
		Destroy(gameObject);
		// TODO spawn particles and stuff
	}
}
