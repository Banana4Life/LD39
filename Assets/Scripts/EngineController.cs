using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EngineController : Engine
{
	private ParticleSystem part;
	private PlayerController player;

	// Use this for initialization
	void Start ()
	{
		part = GetComponent<ParticleSystem>();
		player = GetComponentInParent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		active = player.isThrusting;
		var emission = part.emission;
		emission.enabled = player.isThrusting;
	}
}
