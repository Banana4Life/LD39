using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject mineObject;
    public GameObject baseObject;

    public GameObject lastTarget;

    public Plane activeNavPlane;

    public bool restart;

    public bool active = false;

    // Update is called once per frame
    void Update()
    {    
        var agent = GetComponent<NavMeshAgent>();
        if (!active)
        {
            agent.SetDestination(gameObject.transform.position);
        }
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
        RemoveBlockingAsteroids();
    }

    private void RemoveBlockingAsteroids()
    {
        const int asteroidArea = 1 << 3;
        const int asteroidLayer = 1 << 8;
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(new Vector3(transform.position.x, 0, transform.position.z), out navMeshHit, 1, -1))
        {
            if ((navMeshHit.mask & asteroidArea) == asteroidArea)
            {
                var rb = GetComponent<Rigidbody>();
                if (rb.velocity.sqrMagnitude > 0)
                {
                    var pos = transform.position;
                    pos.y = 1;
                    RaycastHit sphereHit;
                    if (Physics.SphereCast(pos, 1, transform.up, out sphereHit, 1, asteroidLayer))
                    {
                        Destroy(sphereHit.collider.gameObject);
                        if (activeNavPlane)
                        {
                            activeNavPlane.build = true;
                        }
                    }
                }
            }
        }
    }

    public static bool DidAgentReachDestination(NavMeshAgent agent)
    {
        var distance = Vector3.Distance(agent.gameObject.transform.position, agent.destination);
        return distance <= agent.stoppingDistance;
    }

    private void flipTarget(NavMeshAgent agent)
    {
        lastTarget = mineObject == lastTarget ? baseObject : mineObject;
        agent.ResetPath();
        agent.SetDestination(lastTarget.transform.position);
    }

    private void OnCollisionEnter(Collision other)
    {
        var rocket = other.gameObject.GetComponent<RocketController>();
        if (rocket)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var plane = other.gameObject.GetComponent<Plane>();
        if (plane)
        {
            activeNavPlane = plane;
        }
    }
}