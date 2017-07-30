using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int life = 25;

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
        Destroy(gameObject);
    }
    
}