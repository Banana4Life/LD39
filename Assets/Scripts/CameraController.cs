using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public GameObject player;
	private PostProcessingBehaviour postProcessingBehaviour;
	private float tdis;
	private float tvol;

	private void Start()
	{
		postProcessingBehaviour = GetComponent<PostProcessingBehaviour>();
		postProcessingBehaviour.enabled = true;
	}

	void Update ()
	{
		if (player == null)
		{
			return;
		}
		var playerPos = player.transform.position;
		transform.position = new Vector3(playerPos.x, playerPos.y + 10, playerPos.z);
		var playerController = player.GetComponent<PlayerController>();
		postProcessingBehaviour.enabled = playerController.InClouds > 0 && playerController.InCleaner < 0;
		setMusicFilter();
	}

	void setMusicFilter()
	{
		var playerController = player.GetComponent<PlayerController>();
		var backgrundmusic = GetComponent<AudioSource>();
		var filter = GetComponent<AudioDistortionFilter>();
		if (playerController.InClouds > 0 && playerController.InCleaner < 0)
		{
			tdis += Time.deltaTime / 5;
			tvol -= Time.deltaTime / 5;
			if (tdis > 1)
			{
				tdis = 1;
			}
			if (tvol < 0)
			{
				tvol = 0;
			}
			filter.distortionLevel = Mathf.Lerp(0f, 0.9f, tdis);
			backgrundmusic.volume = Mathf.Lerp(0.15f, 1f, tvol);
		}
		else
		{
			tdis -= Time.deltaTime/3;
			tvol += Time.deltaTime / 3;
			if (tdis < 0)
			{
				tdis = 0;
			}
			if (tvol > 1)
			{
				tvol = 1;
			}
			filter.distortionLevel = Mathf.Lerp(0f, 0.9f, tdis);
			backgrundmusic.volume = Mathf.Lerp(0.15f, 1f, tvol);
		}
	}
}
