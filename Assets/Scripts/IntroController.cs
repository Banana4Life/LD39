using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AsyncOperation = UnityEngine.AsyncOperation;

public class IntroController : MonoBehaviour
{
	public int blend = 0;
	public float delta = 0;
	public bool playIntro;
	public float[] blendTimes = {5f, 3f, 7f};
	private Vector3[] movement = {new Vector3(0.02f, 0, -0.03f), new Vector3(-0.03f, -0.03f, -0.05f), new Vector3(-0.01f, 0, -0.02f)};
	public GameObject[] blends;
	public GameObject text;
	public GameObject splashBackground;

	private AsyncOperation loadingOperation;

	void Start()
	{
		loadingOperation = SceneManager.LoadSceneAsync("Main");
		loadingOperation.allowSceneActivation = false;
	}
	
	void Update ()
	{
		if (Input.anyKey)
		{
			playIntro = true;
			blends[blend].SetActive(true);
			text.SetActive(false);
			splashBackground.SetActive(false);
		} 
		else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
		{
			loadingOperation.allowSceneActivation = true;
		}
		if (playIntro)
		{
			blends[blend].transform.position += movement[blend];
			delta += Time.deltaTime;
			if (blend < 3 && delta > blendTimes[blend])
			{
				if (blend < 2)
				{
					blends[blend].SetActive(false);
					blends[blend + 1].SetActive(true);
				}
				else
				{
					loadingOperation.allowSceneActivation = true;
				}
				blend++;
				delta = 0;
			}
		}
	}
}
