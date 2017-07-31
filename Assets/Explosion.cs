using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public bool exploded;

    public AudioClip explosionSound;
    private AudioSource audioSource;
    
    void Awake () {
    
        audioSource = GetComponent<AudioSource>();

    }
    
    public void explode()
    {
        audioSource.PlayOneShot(explosionSound);
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
