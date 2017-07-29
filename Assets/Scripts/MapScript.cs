using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.AI;

public class MapScript : MonoBehaviour
{
	public bool updateMesh;

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
