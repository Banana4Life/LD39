using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
	public Material background;
	public Material debugBackground;

	public bool debug;
	
	// Update is called once per frame
	void Update ()
	{
		GetComponent<MeshRenderer>().material = debug ? debugBackground : background;
	}
}
