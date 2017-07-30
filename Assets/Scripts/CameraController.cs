﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public GameObject player;
	private PostProcessingBehaviour postProcessingBehaviour;
	private Camera cam;
	private int originalCullingMask;

	private void Start()
	{
		postProcessingBehaviour = GetComponent<PostProcessingBehaviour>();
		postProcessingBehaviour.enabled = true;
		cam = GetComponent<Camera>();
		originalCullingMask = cam.cullingMask;
	}

	void Update ()
	{
		var playerPos = player.transform.position;
		transform.position = new Vector3(playerPos.x, playerPos.y + 10, playerPos.z);
		var playerController = player.GetComponent<PlayerController>();
		postProcessingBehaviour.enabled = playerController.InClouds;
		cam.cullingMask = playerController.InClouds ? cam.cullingMask & ~(1 << 13) : originalCullingMask;
	}
}
