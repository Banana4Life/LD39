using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
	public Material debugBackground;

	public bool debug;
	
	// Update is called once per frame
	void Update ()
	{
		if (debug)
		{
			GetComponent<MeshRenderer>().material = debugBackground;
		}
	}
}
