using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EngineController : MonoBehaviour
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
		var emission = part.emission;
		emission.enabled = player.isThrusting;
	}
}
