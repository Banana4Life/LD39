using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int life = 25;
    public GameObject explosion;
    public GameObject leftOver;

    public virtual void Hit(int amount)
    {
        life = Mathf.Max(0, life - amount);
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
        if (leftOver != null)
        {
            var instanceLeftOver = Instantiate(leftOver);
            instanceLeftOver.transform.position = gameObject.transform.position;
        }
        Destroy(gameObject);
    }
    
}
