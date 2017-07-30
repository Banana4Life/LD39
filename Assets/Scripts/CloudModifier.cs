using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CloudModifier : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
	{
		changeAlpha(other.gameObject, .1f);
	}

	private void OnTriggerExit(Collider other)
	{
		changeAlpha(other.gameObject, 1f);
	}

	private static void changeAlpha(GameObject obj, float alpha)
	{
		var emitter = obj.GetComponent<ParticleSystem>();
		var main = emitter.main;
		var startColor = main.startColor;
		var color = startColor.color;
		color.a = alpha;
		startColor.color = color;
		main.startColor = startColor;
	}
}
