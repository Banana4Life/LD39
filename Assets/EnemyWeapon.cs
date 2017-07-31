﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Timers;
using UnityEngine;

public enum WeaponType
{
	LASER,
	SILLYLASER,
	ROCKET,
	GIANTLASER
}

public class EnemyWeapon : MonoBehaviour
{
	public GameObject projectile;
	
	public WeaponType type;
	public int range;

	public float cooldown = 2f;

	[ReadOnly] public float cooldownValue; 
	    
	public AudioSource laserSound;

	private bool playing = false;
	
	// Use this for initialization
	void Start ()
	{
		ParticleSystem ps = gameObject.GetComponentInChildren<ParticleSystem>();
		if (ps)
		{
			cooldown = ps.main.duration;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		SeekPlayer();

		if (type == WeaponType.SILLYLASER)
		{
			var playerPos = GameObject.Find("Player").transform.position;
			var distance = (playerPos - gameObject.transform.position).sqrMagnitude;
			if (distance < range * range)
			{
				if (!playing)
				{
					foreach (var laser in gameObject.GetComponentsInChildren<ParticleSystem>())
					{
						laser.Play();
					}
					playing = true;
				}
			}
			else if (playing)
			{
				foreach (var laser in gameObject.GetComponentsInChildren<ParticleSystem>())
				{
					laser.Stop();
				}
				playing = false;
			}
		}
		else
		{
			cooldownValue -= Time.deltaTime;
			if (cooldownValue <= 0)
			{
				cooldownValue = cooldown;
				Shoot();
			}
		}
	}

	private void Shoot()
	{
		var player = GameObject.Find("Player");
		var playerPos = player.transform.position;
		var distance = (playerPos - gameObject.transform.position).sqrMagnitude;
		if (distance < range * range)
		{
			var laserSound = GetComponent<AudioSource>();
			if (!laserSound.isPlaying)
			{
				laserSound.Play();
			}
			switch (type)
			{
				case WeaponType.LASER:
					foreach (var laser in gameObject.GetComponentsInChildren<ParticleSystem>())
					{
						laser.Play();
					}	
					break;
				case WeaponType.ROCKET:
					gameObject.GetComponentInChildren<ParticleSystem>().Play();
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
			if (laserSound.isPlaying)
			{
				laserSound.Stop();
			}
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
			case WeaponType.SILLYLASER:
				if (distance < range * range)
				{
					gameObject.transform.Rotate(0, 0.25f, 0);
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
