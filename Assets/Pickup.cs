using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

	public PowerUp type;

	private void OnCollisionEnter(Collision other)
	{
		var player = other.gameObject.GetComponent<PlayerController>();
		if (player != null)
		{
			player.pickup(type);
			Destroy(gameObject);
		}
	}
}
