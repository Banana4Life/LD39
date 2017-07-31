using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerbarController : MonoBehaviour {
	private Text powerbarText;
	private PlayerController playerController;
	private float initalMaxPower;
	public GameObject player;
	
	// Use this for initialization
	void Start ()
	{
		powerbarText = GetComponentInChildren<Text>();
		playerController = player.GetComponent<PlayerController>();
		initalMaxPower = playerController.GetShieldPower();
	}
	
	// Update is called once per frame
	void Update ()
	{
		var power = playerController.Power;
		powerbarText.text = Mathf.Round(power / initalMaxPower * 100f) + "%";
	}
}
