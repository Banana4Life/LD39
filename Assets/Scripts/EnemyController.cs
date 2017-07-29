using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject mineObject;
    public GameObject baseObject;

    public GameObject lastTarget;

    public bool restart;

    // Update is called once per frame
    void Update()
    {
        var agent = GetComponent<NavMeshAgent>();
        //var dist = Mathf.CeilToInt(lastTarget.GetComponent<Collider>().bounds.size.y / 2); 
        if (restart)
        {
            flipTarget(agent);
            restart = false;
        }
        else if (DidAgentReachDestination(agent)) //Arrived.
        {
            flipTarget(agent);
        }
    }
    
    public static bool DidAgentReachDestination(NavMeshAgent agent)
    {
        var distance = Vector3.Distance(agent.gameObject.transform.position, agent.destination);
        return distance + 1 <= agent.stoppingDistance;
    }

    private void flipTarget(NavMeshAgent agent)
    {
        lastTarget = mineObject == lastTarget ? baseObject : mineObject;
        agent.ResetPath();
        agent.SetDestination(lastTarget.transform.position);
    }
}