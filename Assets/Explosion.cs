using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public bool exploded;
    
    public void explode()
    {
        var ps = gameObject.GetComponent<ParticleSystem>();
        ps.Play();
        gameObject.transform.parent = null; // Detach from parent to keep playing
    }

    void Update()
    {
        var ps = gameObject.GetComponent<ParticleSystem>();
        if (exploded && !ps.isPlaying)
        {
            Destroy(gameObject); // Destroy when emmiter is done
        }
    }
    
    
}
