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
            gameObject.GetComponentInChildren<Engine>().active = false;
            agent.SetDestination(gameObject.transform.position);
        }
        else 
        {
            GetComponentInChildren<Engine>().gameObject.GetComponent<ParticleSystem>().Play();
            gameObject.GetComponentInChildren<Engine>().active = true;
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
    }

    public override void Hit(int amount)
    {
        var shield = GetComponentInChildren<Shield>();
        if (shield.shielded)
        {
            shield.Hit(amount);
        }
        else
        {
            base.Hit(amount);
        }
    }

    private void RemoveBlockingAsteroids()
    {
        const int asteroidArea = 1 << 3;
        int asteroidLayer = LayerMask.NameToLayer("Asteroids");
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(new Vector3(transform.position.x, 0, transform.position.z), out navMeshHit, 1, -1))
        {
            if ((navMeshHit.mask & asteroidArea) == asteroidArea)
            {
                var rb = GetComponent<Rigidbody>();
                if (rb.velocity.sqrMagnitude > 0)
                {
                    var pos = transform.position;
                    var height = 5f;
                    var lower = transform.position;
                    lower.y -= height;
                    var upper = transform.position;
                    upper.y += height;
                    var colliders = Physics.OverlapCapsule(lower, upper, 1, asteroidLayer);
                    if (colliders.Length > 0)
                    {
                        Collider nearest = null;
                        float nearstMagnitude = float.MaxValue;
                        foreach (var c in colliders)
                        {
                            float magn = (c.transform.position - pos).sqrMagnitude;
                            if (magn < nearstMagnitude)
                            {
                                nearest = c;
                                nearstMagnitude = magn;
                            }
                        }
                        if (nearest != null) {
                            nearest.gameObject.GetComponent<Asteroid>().Destroy();
                            if (activeNavPlane)
                            {
                                activeNavPlane.build = true;
                            }
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