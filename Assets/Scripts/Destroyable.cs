using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int life = 25;
    public GameObject explosion;

    public virtual void Hit(int amount)
    {
        life--;
        if (life == 0)
        {
            Destroy();
        }
    }
	
    public virtual void Destroy()
    {
        if (explosion != null)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity).GetComponent<Explosion>().explode();
        }        
        Destroy(gameObject);
    }
    
}