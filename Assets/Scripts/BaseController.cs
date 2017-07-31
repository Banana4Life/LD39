using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
	public GameObject weapons;
	public GameObject powerUp;
	public Vector3 position;

	private bool spawned = false;
	
	// Update is called once per frame
	void Update () {
		if (weapons.transform.childCount == 0 && !spawned)
		{
			var powerUp = Instantiate(this.powerUp);
			spawned = true;
		}
	}
}
