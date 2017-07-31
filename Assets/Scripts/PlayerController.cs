using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
	
	private bool weaponFiring = false;
	private float deltaLastShot = 0f;

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
		
		var laser = gameObject.GetComponentInChildren<Projectile>();
		var ps = laser.gameObject.GetComponent<ParticleSystem>();
		if (left)
		{
			deltaLastShot += Time.deltaTime;
			var v = GetComponent<Rigidbody>().velocity;
			var rot = transform.forward;
			var main = ps.main;
			main.startSpeed = 5f + Vector3.Project(v, rot).magnitude;
			if (deltaLastShot > firingRate)
			{
				if (ConsumePower(PowerPerShot))
				{
					deltaLastShot -= firingRate;
					ps.Play();
					laserSound.Play();
				}
				else
				{
					deltaLastShot = firingRate;
				}
			}
		}
		else
		{
			deltaLastShot = firingRate;
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
		// TODO game over
	}
	
	public void startInCloud()
	{
		InClouds = 1f;
	}

}
