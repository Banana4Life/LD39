using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
	public bool InClouds;

	public int power = 50; // is running out
	
	public float speed = 3;
	
	public List<GameObject> list;
	[ReadOnly] public int walkTo;

	public bool restart = true;
	public bool autoPilot = true;

	private CloudGenerator cloudsGenerator;

	private void Start()
	{
		InClouds = false;
		cloudsGenerator= GameObject.Find("Clouds").GetComponent<CloudGenerator>();
	}

	// Update is called once per frame
	void Update ()
	{
		doMovement();
		doFire();

		AutoPilot();
		lookToMouse();
		checkIfInClouds();
	}

	private void checkIfInClouds()
	{
		InClouds = cloudsGenerator.HasClouds(transform.position);
	}

	private void doFire()
	{
		var left = Input.GetMouseButton(0);
		var right = Input.GetMouseButton(1);
		
		var laser = gameObject.GetComponentInChildren<ParticleSystem>();
		if (right)
		{
			laser.Play();
		}
		else
		{
			laser.Stop();
		}
	}

	private void doMovement()
	{
		var vert = Input.GetAxisRaw("Vertical");
		var hori = Input.GetAxisRaw("Horizontal");
		GetComponent<Rigidbody>().AddForce(new Vector3(hori, 0, vert).normalized * speed, ForceMode.Impulse);
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

	private void lookToMouse()
	{
		var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		point.y = transform.position.y;
		transform.LookAt(point);
	}
	
}
