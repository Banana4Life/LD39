using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerbarController : MonoBehaviour {
	private Text powerbarText;
	private PlayerController player;
	private float initalMaxPower;
	
	// Use this for initialization
	void Start ()
	{
		powerbarText = GetComponentInChildren<Text>();
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		initalMaxPower = player.GetShieldPower();
	}
	
	// Update is called once per frame
	void Update ()
	{
		var power = player.Power;
		powerbarText.text = Mathf.Round(power / initalMaxPower * 100f) + "%";
	}
}
