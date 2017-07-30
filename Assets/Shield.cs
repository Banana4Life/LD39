using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Shield : Destroyable
{

	public bool shielded;

	public float shieldThickness = 0.2f;

	public float unshielded;

	private CapsuleCollider cc;

	// Use this for initialization
	void Start ()
	{
		cc = gameObject.GetComponent<CapsuleCollider>();
		unshielded = cc.radius;
		cc.radius = 0.1f + cc.radius;
		unhit();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (shielded)
		{
			cc.radius = unshielded + shieldThickness;
		}
		else
		{
			cc.radius = unshielded;
		}
	}

	public override void Hit(int amount)
	{
		base.Hit(amount);
		if (shielded)
		{
			gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;	
			Invoke("unhit", 1f);
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
	}
	
	
}
