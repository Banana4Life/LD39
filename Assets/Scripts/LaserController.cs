using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
	public GameObject source;
	public GameObject player;
	public Color color;
	public float startWidth = 0.05f;
	public float shootWidth = 1.5f;
	public float timeToShoot = 2f;
	
	private GameObject spriteObject;
	private float size;
	private readonly Vector3 rayExtends = new Vector3(0.05f, 10, 0.05f);
	private float time;

	private int state = 0;

	private void Start()
	{
		var renderer = GetComponentInChildren<SpriteRenderer>();
		size = renderer.bounds.extents.y;
		renderer.color = color;
		spriteObject = renderer.gameObject;
		Invoke(nameof(GoState1), timeToShoot);
	}

	void Update ()
	{
		switch (state)
		{
				case 0:
					updateState0();
					break;
				case 1:
					updateState1();
					break;
		}
	}

	private void updateState0()
	{
		time += Time.deltaTime;
		var barrelPos = source.transform.position;
		transform.position = new Vector3(barrelPos.x, transform.position.y, barrelPos.z);
		var pos = transform.position;
		var playerPos = player.transform.position;
		playerPos.y = pos.y;
		var dir = playerPos - pos;
		var distance = dir.magnitude;
		RaycastHit hit;
		if (Physics.BoxCast(pos, rayExtends, dir, out hit, transform.rotation, distance, LayerMask.GetMask("Asteroids")))
		{
			var colliderPos = hit.collider.gameObject.transform.position;
			colliderPos.y = pos.y;
			distance = (colliderPos - pos).magnitude;
		}
		
		Debug.Log("Distance: " + distance);
		spriteObject.transform.localScale = new Vector3(Mathf.Lerp(startWidth, shootWidth, time / timeToShoot), distance / size * 2, 1);
		transform.LookAt(playerPos);
	}

	private void updateState1()
	{
		
	}

	private void GoState1()
	{
		state = 1;
	}

	private void DoShoot()
	{
		Destroy(gameObject);
	}
}
