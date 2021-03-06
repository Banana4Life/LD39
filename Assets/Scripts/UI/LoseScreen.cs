﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour {

	void Start () {
		GameObject.Find("LoseExitButton").GetComponent<Button>().onClick.AddListener(OnExit);
		GameObject.Find("LoseRestartButton").GetComponent<Button>().onClick.AddListener(OnRestart);
	}
	
	private void OnExit()
	{
		Debug.Log("Exit");
		Application.Quit();
	}

	private void OnRestart()
	{
		Debug.Log("Restart");
		SceneManager.LoadScene("Main");
	}
}
