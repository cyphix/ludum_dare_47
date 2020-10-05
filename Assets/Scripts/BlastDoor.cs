using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BlastDoor : MonoBehaviour
{
    public LayerMask playerMask;
    public float collisionRadius = 2.0f; // used for auto open of door
    public bool locked = false;

    private bool doorOpen = false;
    private Animator animator;
    private bool active = true;
    private Collider2D lockedCollider;

    void Start()
    {
        this.animator = this.gameObject.GetComponent<Animator>();
        this.lockedCollider = this.gameObject.GetComponent<Collider2D>();
    }
    
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (this.active)
        {
            if (!this.locked)
            {
                this.doorOpen = Physics2D.OverlapCircle(transform.position, this.collisionRadius, this.playerMask);
            }

            this.animator.SetBool("doorOpen", this.doorOpen);

            // set locked animation if true
            this.animator.SetBool("doorLocked", this.locked);
            //this.lockedCollider.enabled = this.locked;

            // set as trigger if unlocked, otherwise turn off trigger
            this.lockedCollider.isTrigger = !this.locked;

            // turn off the collider if the door is open
            this.gameObject.GetComponent<Collider2D>().enabled = !this.doorOpen;
        }
    }



    public void TurnOff()
    {
        this.active = false;
    }

    public void TurnOn()
    {
        this.active = true;
    }
}
