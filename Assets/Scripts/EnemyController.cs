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
    
    public AudioSource flySound;
    private ParticleSystem particleSystem;
    private Engine engine;

    private void Start()
    {
        engine = GetComponentInChildren<Engine>();
        particleSystem = engine.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {    
        var flySound = GetComponent<AudioSource>();
        var agent = GetComponent<NavMeshAgent>();
        if (!active)
        {
            particleSystem.Pause();
            engine.activeEngine = false;
            agent.SetDestination(transform.position);
        }
        else 
        {
            if (!flySound.isPlaying)
            {
                flySound.Play();
            }
            GetComponentInChildren<Engine>().gameObject.GetComponent<ParticleSystem>().Play();
            gameObject.GetComponentInChildren<Engine>().activeEngine = true;
            particleSystem.Play();
            engine.activeEngine = true;
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
        //flySound.Stop();
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