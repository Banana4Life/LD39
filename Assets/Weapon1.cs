using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1 : MonoBehaviour {

	public ParticleSystem part;
	public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

	void Start()
	{
		part = GetComponent<ParticleSystem>();
	}

	void OnParticleCollision(GameObject other)
	{
		Asteroid asteroid = other.GetComponent<Asteroid>();
		if (asteroid != null)
		{
			asteroid.Hit();
		}
	}
}
