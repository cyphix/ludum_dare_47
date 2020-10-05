using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class IchorBlastDoor : MonoBehaviour
{
    public Timer timer;
    public GameObject doorObject;
    public int manifestPhase = 5;
    public int health = 50;
    public bool ichorWall = false;
    public Collider2D ichorWallCollider;

    private SpriteRenderer doorSprite;
    private SpriteRenderer ichorSprite;
    private Collider2D ichorCollider;

    private bool doomEffectActive = false;

    void Start()
    {
        this.doorSprite = this.doorObject.GetComponent<SpriteRenderer>();
        this.ichorSprite = this.gameObject.GetComponent<SpriteRenderer>();

        this.ichorCollider = this.gameObject.GetComponent<Collider2D>();

        this.doorSprite.enabled = true;
        this.ichorSprite.enabled = false;
        this.ichorCollider.enabled = false;
    }
    
    void Update()
    {
    }

    void FixedUpdate()
    {
        // check if in phase
        if (this.timer.GetPhase() <= this.manifestPhase)
        {
            this.doorSprite.enabled = false;
            this.ichorSprite.enabled = true;
            this.ichorCollider.enabled = true;
            if(this.ichorWallCollider != null)
            {
                this.ichorWallCollider.enabled = this.ichorWall;
            }

            this.doorObject.GetComponent<BlastDoor>().TurnOff();

            this.doomEffectActive = true;
        }
        else
        {
            this.doorSprite.enabled = true;
            this.ichorSprite.enabled = false;
            this.ichorCollider.enabled = false;
            if (this.ichorWallCollider != null)
            {
                this.ichorWallCollider.enabled = false;
            }

            this.doorObject.GetComponent<BlastDoor>().TurnOn();

            this.doomEffectActive = false;
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.doomEffectActive)
        {
            if (other.tag == "Player" && !this.ichorWall)
            {
                other.gameObject.GetComponent<PlayerController>().HurtPlayer(
                    30,
                    other.gameObject.transform.position.x - this.transform.position.x
                );
            }
        }
    }



    public bool DoomEffectActive()
    {
        return this.doomEffectActive;
    }

    public void TakeDamage(int damageAmount)
    {
        if (!this.ichorWall)
        {
            // play take damage
            // TODO: take damage animation

            if (this.health - damageAmount <= 0)
            {
                this.Kill();
            }
            else
            {
                this.health -= damageAmount;
            }
        }
    }

    public void Kill()
    {
        // play death animation
        // TODO: death animation

        // all door instances are destroyed
        Destroy(this.doorObject, 0.1f);
        Destroy(this.gameObject, 0.1f);
        Destroy(this, 0.1f);
    }
}
