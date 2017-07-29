using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapScript : MonoBehaviour
{
	public bool updateMesh;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (updateMesh)
		{
			var navMeshSurface = GetComponent<NavMeshSurface>();
			navMeshSurface.BuildNavMesh();
			updateMesh = false;
		}
	}
}
