using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerbarController : MonoBehaviour {
	private Text powerbarText;
	private PlayerController player;

	// Use this for initialization
	void Start ()
	{
		powerbarText = GetComponentInChildren<Text>();
		player = GameObject.Find("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		var max = player.MaxPower;
		var power = player.Power;
		powerbarText.text = Mathf.Round(power / (float) max * 100f) + "%";
	}
}
