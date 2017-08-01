using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public GameObject weapons;
    public GameObject powerUp;
    public PowerUp powerUpType;

    private bool spawned = false;
    [ReadOnly] public ArrowController arrow;

    // Update is called once per frame
    void Update()
    {
        if (weapons.transform.childCount == 0 && !spawned)
        {
            if (this.powerUp != null)
            {
                var powerUp = Instantiate(this.powerUp);
                powerUp.GetComponent<Pickup>().type = powerUpType;
                spawned = true;
                arrow.target = powerUp;
                
            }
            if (powerUpType == PowerUp.END)
            {
                foreach (var exp in gameObject.GetComponentsInChildren<Explosion>())
                {
                    exp.explode();
                }

                Destroy(gameObject);
                GameObject.Find("Player").GetComponent<PlayerController>().Win();
            }
        }
    }
}