using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int life = 25;
    public GameObject explosion;

    public virtual void Hit(int amount)
    {
        life -= amount;
        if (life <= 0)
        {
            Destroy();
        }
    }
	
    public virtual void Destroy()
    {
        if (explosion != null)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.Euler(-90,0,0)).GetComponent<Explosion>().explode();
        }        
        Destroy(gameObject);
    }
    
}
