using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Shield : Destroyable
{

	public bool shielded;

	// Use this for initialization
	void Start ()
	{
		unhit();
	}
	
	public override void Hit(int amount)
	{
		base.Hit(amount);
		if (shielded)
		{
			gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;	
			Invoke(nameof(unhit), 1f);
		}
	}

	public void unhit()
	{
		gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
	}

	public override void Destroy()
	{
		Debug.Log(GetType().Name + " Shield deactivated");
		shielded = false;
		GetComponent<Collider>().enabled = false;
		var particle = gameObject.GetComponentInChildren<ParticleSystem>();
		if (particle != null)
		{
			particle.Play();
		}
	}
	
	
}
