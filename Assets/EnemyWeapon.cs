using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Timers;
using UnityEngine;

public enum WeaponType
{
	LASER
}

public class EnemyWeapon : MonoBehaviour
{

	public WeaponType type;
	public int range;

	public int cooldown = 2;

	[ReadOnly] public float cooldownValue; 
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		SeekPlayer();

		cooldownValue -= Time.deltaTime;
		if (cooldownValue <= 0)
		{
			cooldownValue = cooldown;
			Shoot();
		}
	}

	private void Shoot()
	{
		var playerPos = GameObject.Find("Player").transform.position;
		var distance = (playerPos - gameObject.transform.position).sqrMagnitude;
		if (distance < range * range)
		{
			var laser = gameObject.GetComponentInChildren<ParticleSystem>();
			if (laser != null)
			{
				laser.Play();
				Invoke("stopShoot", 0.5f);	
			}
		}
	}

	private void stopShoot()
	{
		var laser = gameObject.GetComponentInChildren<ParticleSystem>();
		laser.Stop();
	}

	private void SeekPlayer()
	{
		var playerPos = GameObject.Find("Player").transform.position;
		var distance = (playerPos - gameObject.transform.position).sqrMagnitude;
		if (distance < range * range)
		{
			playerPos.y = gameObject.transform.position.y;
			gameObject.transform.LookAt(playerPos);
		}
	}
}
