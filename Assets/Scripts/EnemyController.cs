using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking.NetworkSystem;

public class EnemyController : Destroyable
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
            GetComponentInChildren<Engine>().gameObject.GetComponent<ParticleSystem>().Pause();
            gameObject.GetComponentInChildren<Engine>().activeEngine = false;
            agent.SetDestination(gameObject.transform.position);
        }
        else 
        {
            GetComponentInChildren<Engine>().gameObject.GetComponent<ParticleSystem>().Play();
            gameObject.GetComponentInChildren<Engine>().activeEngine = true;
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
    }

    public override void Hit(int amount)
    {
        var shield = GetComponentInChildren<Shield>();
        if (shield != null && shield.shielded)
        {
            shield.Hit(amount);
        }
        else
        {
            base.Hit(amount);
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
            return;
        }
        var asteriod = other.gameObject.GetComponent<Asteroid>();
        if (asteriod)
        {
            asteriod.Destroy();
            return;
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

    public override void Destroy()
    {
        
        foreach (var ps in GetComponentsInChildren<Explosion>())
        {
            ps.explode();
        }
        
        base.Destroy();
    }
}