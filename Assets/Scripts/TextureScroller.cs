using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureScroller : MonoBehaviour
{
	public float ScrollSpeed = 1;
	private GameObject player;

	private void Start()
	{
		player = GameObject.Find("Player");
	}

	void Update ()
	{
		if (player == null)
		{
			return;
		}
		Material mat = GetComponent<Renderer>().material;
		var currentOffset = mat.GetTextureOffset("_MainTex");

		var v = player.GetComponent<Rigidbody>().velocity;
		var dir = new Vector2(v.x, v.z) * -1;
		mat.SetTextureOffset("_MainTex", currentOffset + dir * Time.deltaTime * ScrollSpeed);
	}
}
