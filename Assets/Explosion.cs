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
        exploded = true;
    }

    void Update()
    {
        var ps = gameObject.GetComponent<ParticleSystem>();
        if (exploded && !ps.IsAlive())
        {
            Destroy(gameObject); // Destroy when emmiter is done
        }
    }
    
    
}
