using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.AI;

public class MapScript : MonoBehaviour
{
	public bool updateMesh;

	public GameObject backgroundMap;
	public GameObject plane;
	public int distance;
	public int size;

	private void OnAsteriodsGenerated()
	{
		for (int x = 0; x < size; x++)
		{
			for (int z = 0; z < size; z++)
			{
				var planeInstance = Instantiate(plane, new Vector3(x*distance, 0, z*distance), Quaternion.identity, gameObject.transform);
				planeInstance.name = "Plane " + x + ":" + z;
			}
				
		}
		UpdateAllNavMeshes();
		GameObject.Find("Entities").SendMessage("OnNavMeshReady");
	}

	// Update is called once per frame
	void Update ()
	{
		if (updateMesh)
		{
			UpdateAllNavMeshes();
			updateMesh = false;
		}
	}

	public void UpdateAllNavMeshes()
	{
		var navMeshSurface = GetComponentsInChildren<Plane>();
		foreach (var meshSurface in navMeshSurface)
		{
			meshSurface.UpdateNavMesh();
		}
	}
}
