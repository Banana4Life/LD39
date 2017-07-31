using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PowerUp
{
	PLASMA,
	ROCKET,
	LASER,
	END
}

public class PlayerController : Destroyable
{
	public float InClouds;
	public float InCleaner;

	public float Power = 500; // is running out
	public float PowerRegain = 40f;
	public float PowerRegainInClouds = 5f;
	public float PowerPerShot = 2f;
	
	public float speed = 3;
	
	public List<GameObject> list;
	[ReadOnly] public int walkTo;

	public bool restart = true;
	public bool autoPilot = true;
	public bool isThrusting;

	private Shield shield;

	public AudioSource laserSound;
	public AudioSource flySound;

	public float firingRate = 0.2f;
	public float firingRateDual = 0.07f;
	public float firingRateRocket = 1f;
	
	private bool weaponFiring = false;
	private float deltaLastShot = 0f;
	private float deltaLastRocket = 0f;

	public bool powerDualLaser;
	public bool powerRocket;
	public bool powerGiantLaser;
	
	public GameObject rocketPrefab;

	[ReadOnly] public bool leftLaser;

	public GameObject plasma1;
	public GameObject plasma2;

	private void Start()
	{
		InClouds = 0f;
		shield = GetComponentInChildren<Shield>();
		Power = GetShieldPower();
	}

	// Update is called once per frame
	void Update ()
	{
		doMovement();
		doFire();

		AutoPilot();
		lookToMouse();
		reGenPower();

		InClouds -= Time.deltaTime;
		InCleaner -= Time.deltaTime;
	}

	public bool ConsumePower(float required = 1)
	{
		if (Power > required)
		{
			Power -= required;
			return true;
		}
		return false;
	}

	public float GetShieldPower()
	{
		if (!shield)
		{
			shield = GetComponentInChildren<Shield>();
		}
		return shield.life;
	}

	private void reGenPower()
	{
		var gain = InClouds > 0 ? PowerRegainInClouds : PowerRegain;
		Power = Mathf.Min(GetShieldPower(), Power + gain * Time.deltaTime);
	}

	private void doFire()
	{
		var left = Input.GetMouseButton(0);
		var right = Input.GetMouseButton(1);
		
		deltaLastShot += Time.deltaTime;
		deltaLastRocket += Time.deltaTime;
		
		if (left)
		{
			var v = GetComponent<Rigidbody>().velocity;
			var rot = transform.forward;
		
			if (deltaLastShot > (powerDualLaser ? firingRateDual : firingRate))
			{
				if (!powerDualLaser)
				{
					var laser = plasma1.GetComponentInChildren<Projectile>();
					var ps = laser.gameObject.GetComponent<ParticleSystem>();
					var main = ps.main;
					main.startSpeed = 5f + Vector3.Project(v, rot).magnitude;
					if (ConsumePower(PowerPerShot))
					{
						deltaLastShot = 0;
						ps.Play();
						laserSound.Play();
					}
				}
				else
				{
					ParticleSystem ps = null;
					var lasers = plasma2.GetComponentsInChildren<Projectile>();
					int i = 0;
					foreach (var laser in lasers)
					{
						if (leftLaser && i == 0)
						{
							ps = laser.gameObject.GetComponent<ParticleSystem>();							
						}
						else if (!leftLaser &&  i == 1)
						{
							ps = laser.gameObject.GetComponent<ParticleSystem>();
						}
						i++;
					}
					if (ps != null)
					{
						var main = ps.main;
						main.startSpeed = 5f + Vector3.Project(v, rot).magnitude;
						if (ConsumePower(PowerPerShot))
						{
							deltaLastShot = 0;
							ps.Play();
							laserSound.Play();
						}
					}
					leftLaser = !leftLaser;						
				}
			}
		}
		if (right && powerRocket)
		{
			if (deltaLastShot > (powerDualLaser ? firingRateDual : firingRate) && deltaLastRocket > firingRateRocket)
			{
				var pos = plasma1.gameObject.transform.position;
				pos.y += 0.5f;
				Instantiate(rocketPrefab, pos, gameObject.transform.rotation, GameObject.Find("Projectiles").transform);
				deltaLastRocket = 0;
			}
		}
	}

	private void doMovement()
	{
		var vert = Input.GetAxisRaw("Vertical");
		var hori = Input.GetAxisRaw("Horizontal");

		isThrusting = Mathf.Abs(vert) + Mathf.Abs(hori) > 0;

		var dir = transform.forward * vert + transform.right * hori;
		GetComponent<Rigidbody>().AddForce(new Vector3(dir.x, 0, dir.z).normalized * speed * Time.deltaTime, ForceMode.Impulse);
		if (isThrusting && !flySound.isPlaying)
		{
			flySound.Play();
		}
		
		if (!isThrusting && flySound.isPlaying)
		{
			flySound.Stop();
		}
		
	}

	private void AutoPilot()
	{
		var agent = GetComponent<NavMeshAgent>();
		agent.enabled = autoPilot;
		if (autoPilot)
		{
			if (restart)
			{
				nextTarget(agent);
				restart = false;
			}
			else if (EnemyController.DidAgentReachDestination(agent)) //Arrived.
			{
				nextTarget(agent);
			}
		}
	}

	private void nextTarget(NavMeshAgent agent)
	{
		walkTo++;
		if (walkTo >= list.Count)
		{
			walkTo = 0;
		}
		agent.ResetPath();
		agent.SetDestination(list[walkTo].transform.position);
	}

	public override void Hit(int amount)
	{
		if (shield.shielded)
		{
			shield.Hit(amount);
		}
		else
		{
			base.Hit(amount);
		}
	}

	private void lookToMouse()
	{
		var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		point.y = transform.position.y;
		transform.LookAt(point);
	}

	public override void Destroy()
	{
		base.Destroy();
		Invoke(nameof(GameOver), 1f);
	}

	private void GameOver()
	{
		
	}
	
	public void startInCloud()
	{
		InClouds = 1f;
	}

	public void pickup(PowerUp powerUp)
	{
		switch (powerUp)
		{
			case PowerUp.PLASMA:
				powerDualLaser = true;
				break;
			case PowerUp.ROCKET:
				powerRocket = true;
				break;
			case PowerUp.LASER:
				powerGiantLaser = true;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(powerUp), powerUp, null);
		}
	}

}
