using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Plane : MonoBehaviour
{

	public bool build;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (build)
		{
			GetComponent<NavMeshSurface>().BuildNavMesh();
			build = false;
		}
	}

	public void UpdateNavMesh()
	{
		GetComponent<NavMeshSurface>().BuildNavMesh();	
	}
}
