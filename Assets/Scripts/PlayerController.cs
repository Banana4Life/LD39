using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
	public float speed = 3;
	
	public List<GameObject> list;
	[ReadOnly] public int walkTo;

	public bool restart = true;
	public bool autoPilot = true;
	
	// Update is called once per frame
	void Update ()
	{
		var vert = Input.GetAxisRaw("Vertical");
		var hori = Input.GetAxisRaw("Horizontal");
		GetComponent<Rigidbody>().AddForce(new Vector3(hori, 0, vert).normalized * speed, ForceMode.Impulse);
		
		var agent = GetComponent<NavMeshAgent>();
		agent.enabled = autoPilot;
		//var dist = Mathf.CeilToInt(lastTarget.GetComponent<Collider>().bounds.size.y / 2); 
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

	private void nextTarget(NavMeshAgent agent)
	{
		walkTo++;
		if (walkTo > list.Count)
		{
			walkTo = 0;
		}
		agent.ResetPath();
		agent.SetDestination(list[walkTo].transform.position);
	}
}
