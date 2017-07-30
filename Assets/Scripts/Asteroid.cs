using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
	public int life = 25;
	
	void Start () {
		// rotate randomly
		transform.rotation = Quaternion.Euler(0f, Random.value * 90f, 0f);
	}

	public void Hit(int amount)
	{
		life--;
		if (life == 0)
		{
			Explode();
		}
	}
	
	public void Explode()
	{
		Debug.Log("Asteroid exploded");
		Destroy(gameObject);
		// TODO spawn particles and stuff
	}
}
