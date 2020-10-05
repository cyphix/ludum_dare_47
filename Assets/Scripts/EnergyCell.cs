using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCell : MonoBehaviour
{
    public float collisionRadius = 0.2f;

    public int energyContained = 50;

    void Start()
    {
    }
    
    void Update()
    {
    }

    void FixedUpdate()
    {
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if(player != null)
            {
                player.GiveEnergy(this.energyContained);

                // item used, destroy
                Destroy(this.gameObject);
            }
        }
    }
}
