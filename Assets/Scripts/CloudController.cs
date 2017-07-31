using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CloudController : MonoBehaviour
{
	private ParticleSystem part;
	private PlayerController player;
	
	void Start ()
	{
		part = GetComponent<ParticleSystem>();
		part.trigger.SetCollider(0, GameObject.Find("CloudTracker").transform);
	}

	private void OnParticleTrigger()
	{
		handleCollision(GameObject.Find("Player"));
	}
//
//	private void OnParticleCollision(GameObject other)
//	{
//		handleCollision(other);
//	}
//
	private void handleCollision(GameObject other)
	{
		var parts = new List<ParticleSystem.Particle>();
		var particleCount = part.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, parts);
		if (particleCount > 0)
		{
			other.GetComponent<PlayerController>().startInCloud();
		}
	}
}
