using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Timers;
using UnityEngine;

public enum WeaponType
{
	LASER,
	BIGLASER,
	ROCKET
}

public class EnemyWeapon : MonoBehaviour
{
	public GameObject projectile;
	
	public WeaponType type;
	public int range;

	public float cooldown = 2f;
	public float shootDuration = 0.5f;

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
		var player = GameObject.Find("Player");
		var playerPos = player.transform.position;
		var distance = (playerPos - gameObject.transform.position).sqrMagnitude;
		if (distance < range * range)
		{
			switch (type)
			{
				case WeaponType.LASER:
					foreach (var laser in gameObject.GetComponentsInChildren<ParticleSystem>())
					{
						laser.Play();
					}
					Invoke("stopShoot", shootDuration);	
					break;
				case WeaponType.BIGLASER:
					foreach (var laser in gameObject.GetComponentsInChildren<ParticleSystem>())
					{
						laser.Play();
					}
					Invoke("stopShoot", shootDuration);	
					break;
				case WeaponType.ROCKET:
					var rocket = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, GameObject.Find("Projectiles").transform);
					var rocketController = rocket.GetComponent<RocketController>();
					rocketController.target = player;
					rocketController.speed = 1;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	private void stopShoot()
	{
		foreach (var laser in gameObject.GetComponentsInChildren<ParticleSystem>())
		{
			laser.Stop();
		}
	}

	private void SeekPlayer()
	{
		var playerPos = GameObject.Find("Player").transform.position;
		var distance = (playerPos - gameObject.transform.position).sqrMagnitude;
		switch (type)
		{
			case WeaponType.LASER:
				if (distance < range * range)
				{
					playerPos.y = gameObject.transform.position.y;
					gameObject.transform.LookAt(playerPos);
				}
				break;
			case WeaponType.BIGLASER:
				if (distance < range * range)
				{
					gameObject.transform.Rotate(0, 1, 0);
				}
				break;
			case WeaponType.ROCKET:
				// Nothing
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		
	}
}
