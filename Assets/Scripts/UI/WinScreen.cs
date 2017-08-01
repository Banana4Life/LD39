using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {

	void Start () {
		GameObject.Find("WinExitButton").GetComponent<Button>().onClick.AddListener(OnExit);
		GameObject.Find("WinRestartButton").GetComponent<Button>().onClick.AddListener(OnRestart);
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
