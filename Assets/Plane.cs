using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class Plane : MonoBehaviour
{
	public bool build;
	
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
