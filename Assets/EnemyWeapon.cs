using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
	LASER
}

public class EnemyWeapon : MonoBehaviour
{

	public WeaponType type;
	public int range;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		var playerPos = GameObject.Find("Player").transform.position;
		var distance = (playerPos - gameObject.transform.position).sqrMagnitude;
		if (distance < range * range)
		{
			playerPos.y = gameObject.transform.position.y;
			gameObject.transform.LookAt(playerPos);
		}
	}
}
