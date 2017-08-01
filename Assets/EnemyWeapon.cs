using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
	public Color color = Color.white;

	public float cooldown = 2f;

	public float cooldownValue; 
	    
	private GameObject playerObject;
	private AudioSource sound;
	private bool playing;
	private GameObject currentProjectile;

	public int cycleCount = 0;
	public float burstTime = 0;
	public float deltaBurstStart = 0;
	public int soundsPlayed = 1;
	// Use this for initialization
	void Start ()
	{
		playerObject = GameObject.Find("Player");
		ParticleSystem ps = gameObject.GetComponentInChildren<ParticleSystem>();
		if (ps)
		{
			cooldown = ps.main.duration;
		}
		if (type == WeaponType.LASER)
		{
			ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[1];
			gameObject.GetComponentInChildren<ParticleSystem>().emission.GetBursts(bursts);
			cycleCount = bursts[0].cycleCount;
			burstTime = bursts[0].repeatInterval;
		}
		sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerObject == null)
		{
			return;
		}
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
		if (type == WeaponType.LASER && playing)
		{
			deltaBurstStart += Time.deltaTime;
			if (deltaBurstStart > soundsPlayed * burstTime)
			{
				var laserSound = GetComponent<AudioSource>();
				laserSound.Play();
				soundsPlayed++;
				playing = soundsPlayed < cycleCount;
			}
		}
	}

	private void playSound()
	{
		if (!sound.isPlaying)
		{
			sound.Play();
		}
	}

	private void stopSound()
	{
		if (sound.isPlaying)
		{
			sound.Stop();
		}
	}

	private void Shoot()
	{
		var playerPos = playerObject.transform.position;
		var distance = (playerPos - gameObject.transform.position).sqrMagnitude;
		if (distance < range * range)
		{
			playSound();
			switch (type)
			{
				case WeaponType.LASER:
					shootLaser();
					break;
				case WeaponType.SILLYLASER:
					shootSillyLaser();
					break;
				case WeaponType.ROCKET:
					shootRocket();
					break;
				case WeaponType.GIANTLASER:
					shootGiantLaser();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	private void shootLaser()
	{
		foreach (var laser in gameObject.GetComponentsInChildren<ParticleSystem>())
		{
			laser.Play();
			playing = true;
			soundsPlayed = 1;
			deltaBurstStart = 0;
		}	
	}

	private void shootSillyLaser()
	{
		shootLaser();
	}

	private void shootRocket()
	{
		gameObject.GetComponentInChildren<ParticleSystem>().Play();
		var rocket = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, GameObject.Find("Projectiles").transform);
		var rocketController = rocket.GetComponent<RocketController>();
		rocketController.target = playerObject;
		rocketController.speed = 1;
	}

	private void shootGiantLaser()
	{
		var laser = Instantiate(projectile);
		var laserController = laser.GetComponent<LaserController>();
		var barrel = GetComponentInChildren<Barrel>().gameObject;
		laserController.source = barrel;
		laserController.player = playerObject;
		laserController.color = color;
		var pos = barrel.transform.position;
		pos.y++;
		laser.transform.position = pos;

		var playerPos = playerObject.transform.position;
		playerPos.y = pos.y;
		
		laser.transform.LookAt(playerPos);
		currentProjectile = laser;
	}

	private void stopShoot()
	{
		stopSound();
		foreach (var partSys in gameObject.GetComponentsInChildren<ParticleSystem>())
		{
			partSys.Stop();
		}
	}

	private void SeekPlayer()
	{
		var playerPos = playerObject.transform.position;
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
			case WeaponType.GIANTLASER:
				if (currentProjectile == null || !currentProjectile.GetComponent<LaserController>().IsLocked())
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
