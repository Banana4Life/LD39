using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 3;
	
	// Update is called once per frame
	void Update ()
	{
		var vert = Input.GetAxisRaw("Vertical");
		var hori = Input.GetAxisRaw("Horizontal");
		GetComponent<Rigidbody>().AddForce(new Vector3(hori, 0, vert).normalized * speed, ForceMode.Impulse);
	}
}
