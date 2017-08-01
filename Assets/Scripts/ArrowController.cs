using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
	public GameObject target;

	public List<GameObject> targets;
	public int index;

	void Update ()
	{
		if (target == null)
		{
			if (targets.Count == 0)
			{
				Destroy(gameObject);
			}
			target = targets[index++];
			var baseCtrl = target.GetComponent<BaseController>();
			if (baseCtrl != null)
			{
				baseCtrl.arrow = this;
			}
		}
		var targetPos = target.transform.position;
//		targetPos.y = transform.position.y;
		transform.LookAt(targetPos);
	}
}
