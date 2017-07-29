using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject player;

	// Update is called once per frame
	void Update ()
	{
		var playerPos = player.transform.position;
		transform.position = new Vector3(playerPos.x, playerPos.y + 10, playerPos.z);
	}
}
