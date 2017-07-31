using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

	public GameObject target;

	void Update ()
	{
		var targetPos = target.transform.position;
//		targetPos.y = transform.position.y;
		transform.LookAt(targetPos);
	}
}
