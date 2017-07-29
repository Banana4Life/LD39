using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
	public GameObject target;
	public float speed;

	// Update is called once per frame
	void Update () {
		if (target)
		{
			var dir = target.transform.position - transform.position;
			dir.y = 0;
			transform.localPosition += dir.normalized * speed;
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		Debug.Log("collision enter");
		Destroy(gameObject);
	}

	private void OnCollisionStay(Collision other)
	{
		Debug.Log("collision stay");
	}
}
