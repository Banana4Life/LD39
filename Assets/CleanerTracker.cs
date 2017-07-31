using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerTracker : MonoBehaviour {
	
	
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.GetComponent<CloudCleaner>() != null)
		{
			transform.GetComponentInParent<PlayerController>().InCleaner = 0.5f;
		}
	}
}
