using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour {

	public Texture2D cloudMap;
	public GameObject cloud;
	
	// Use this for initialization
	void Start () {
		generateClouds(GameObject.Find("Asteroids"));
	}
	
	public void generateClouds(GameObject astroidContainer)
	{
		
		var distScript = astroidContainer.GetComponent<RandomDistributionScript>();
		int i = 0;
		for (var x = 0; x < distScript.bounds.x; x+=2)
		{
			for (var z = 0; z < distScript.bounds.z; z+=2)
			{
				if (hasClouds(new Vector2(x, z), cloudMap, astroidContainer))
				{
					var aCloud = Instantiate(cloud, gameObject.transform);
					aCloud.transform.localPosition = new Vector3(x, 2, z);
					aCloud.name = "Cloud " + x + ":" + z + "(" + i++ + ")";
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
