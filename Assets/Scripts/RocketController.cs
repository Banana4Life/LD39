using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
	public GameObject target;
	public float speed;
	public bool JumpToTargetLevel;

	// Update is called once per frame
	void Update () {
		if (target)
		{
			var dir = target.transform.position - transform.position;
			dir.y = 0;
			var newPos = transform.position + dir.normalized * speed;
			if (JumpToTargetLevel)
			{
				newPos.y = target.transform.position.y;
			}
			transform.position = newPos;
			var targetPos = target.transform.position;
			targetPos.y = newPos.y;
			transform.LookAt(targetPos);
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
