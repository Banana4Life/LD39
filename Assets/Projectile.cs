using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public ParticleSystem part;
	public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

	void Start()
	{
		part = GetComponent<ParticleSystem>();
	}

	void OnParticleCollision(GameObject other)
	{
		Destroyable destroyable = other.GetComponent<Destroyable>();
		if (destroyable != null)
		{
			int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
			destroyable.Hit(numCollisionEvents);
		}
	}
}
