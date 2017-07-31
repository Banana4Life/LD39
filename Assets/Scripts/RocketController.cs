using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : Destroyable
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
			var newPos = transform.position + dir.normalized * speed * Time.deltaTime;
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
		explodeWith(other);
	}

	private void explodeWith(Collision other)
	{
		var destroyable = other.gameObject.GetComponent<Destroyable>();
		if (destroyable != null)
		{
			destroyable.Hit(200);
		}
		Destroy();
	}

	private void OnCollisionStay(Collision other)
	{
		explodeWith(other);
	}
}
