using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.AI;

public class MapScript : MonoBehaviour
{
	public bool updateMesh;
	public Vector2 dimension = new Vector2(5,5);

	public GameObject asteroid;
	
	// Use this for initialization
	void Start ()
	{
		var terrain = gameObject.GetComponentInChildren<Terrain>();
		var splatmapData = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);
		
		var mTerrainData = terrain.terrainData;
		var alphamapWidth = mTerrainData.alphamapWidth;
		var alphamapHeight = mTerrainData.alphamapHeight;
 
		var mSplatmapData = mTerrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
		var numTextures = mSplatmapData.Length / (alphamapWidth * alphamapHeight);

		for (var x = 0; x < dimension.x; x++)
		{
			for (var z = 0; z < dimension.y; z++)
			{
				var tex = GetActiveTerrainTextureIdx(splatmapData, numTextures, new Vector3(x, 0, z));
				Debug.Log("Tex " + tex);
			}
		}
	}
	
	
	private Vector3 ConvertToSplatMapCoordinate(Vector3 pos)
	{
		Vector3 vecRet = new Vector3();
		Terrain ter = Terrain.activeTerrain;
		Vector3 terPosition = ter.transform.position;
		vecRet.x = ((pos.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
		vecRet.z = ((pos.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
		return vecRet;
	}
   
	
	int GetActiveTerrainTextureIdx(float[,,] splatmapData, int numTextures, Vector3 at)
	{
		Vector3 TerrainCord = ConvertToSplatMapCoordinate(at);
		int ret = 0;
		float comp = 0f;
		for (int i = 0; i < numTextures; i++)
		{
			if (comp < splatmapData[(int)TerrainCord.z, (int)TerrainCord.x, i])
				ret = i;
		}
		return ret;
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
