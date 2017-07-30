using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int life = 25;

    public void Hit(int amount)
    {
        life--;
        if (life == 0)
        {
            Explode();
        }
    }
	
    public void Explode()
    {
        Debug.Log(GetType().Name + " exploded");
        Destroy(gameObject);
        // TODO spawn particles and stuff
    }
    
}