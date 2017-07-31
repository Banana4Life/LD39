using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{

	public float speed = 3;

	// Update is called once per frame
	void Update ()
	{
		transform.position += transform.forward * speed * Time.deltaTime;
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
		Destroy(gameObject);
	}

	private void OnCollisionStay(Collision other)
	{
		explodeWith(other);
	}
}
