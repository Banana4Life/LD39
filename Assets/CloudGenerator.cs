using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour {

	public Texture2D cloudMap;
	public GameObject cloud;
	
	// Use this for initialization
	void Start () {
		generateClouds(gameObject.transform.parent.gameObject);
	}
	
	public void generateClouds(GameObject map)
	{
		var distScript = map.GetComponentInChildren<RandomDistributionScript>();
		
		for (var x = 0; x < distScript.bounds.x; x++)
		{
			for (var z = 0; z < distScript.bounds.z; z++)
			{
				if (hasClouds(new Vector2(x, z), cloudMap, map))
				{
					Debug.Log("Clouds here " + x + ":" + z);
					var aCloud = Instantiate(cloud, map.transform);
					aCloud.transform.localPosition = new Vector3(x, 2, z);
				}
			}
		}
	}

	private static bool hasClouds(Vector2 mapPos, Texture2D clouds, GameObject map)
	{
		var distScript = map.GetComponentInChildren<RandomDistributionScript>();
		int x = Mathf.FloorToInt(mapPos.x / distScript.bounds.x * clouds.width);
		int z = Mathf.FloorToInt(mapPos.y / distScript.bounds.z * clouds.height);
		var pixel = clouds.GetPixel(x, z);
		return pixel.a > 0;
	}
}
