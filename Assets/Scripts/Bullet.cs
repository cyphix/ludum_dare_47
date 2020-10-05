using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public float timeToLive = 2.0f;
    public int damage = 10;
    public float direction = 1; // left -1/1 right
    public float destroyAnimationDuration = 0.1f;

    private Rigidbody2D bulletRigidBody;
    private Collider2D bulletCollider;
    private SpriteRenderer bulletRenderer;

    private Animator animator;

    private bool startCalled = false;

    void Start()
    {
        this.bulletRigidBody = GetComponent<Rigidbody2D>();
        this.bulletCollider = GetComponent<Collider2D>();

        this.bulletRenderer = GetComponent<SpriteRenderer>();

        this.animator = GetComponent<Animator>();

        if (this.direction < 0)
        {
            this.bulletRenderer.flipX = true;
        }

        this.startCalled = true;
    }
    
    void Update()
    {
    }

    void FixedUpdate()
    {
        this.bulletRigidBody.AddForce(
            transform.right * this.direction * this.speed
        );

        // calculate time to live, destroy when over
        this.timeToLive -= Time.deltaTime;
        if(this.timeToLive <= 0.0f)
        {
            this.DestroyBullet();
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy" && other.gameObject.name == "Ichor")
        {
            IchorBlastDoor blastDoor = other.gameObject.GetComponent<IchorBlastDoor>();

            if(blastDoor != null)
            {
                if (blastDoor.DoomEffectActive())
                {
                    blastDoor.TakeDamage(this.damage);

                    this.DestroyBullet();
                }
            }
        }
        else if (other.tag == "Environment")
        {
            this.DestroyBullet();
        }
    }



    public void DestroyBullet()
    {
        if(!this.startCalled)
        {
            // initialize if trigger fires before it starts
            this.Start();
        }
        
        // stop collider so it does not keep triggering
        this.bulletCollider.enabled = false;
        
        // stop bullet travel
        this.bulletRigidBody.velocity = new Vector2(0.0f, 0.0f);
        
        // play destroy
        this.animator.SetTrigger("destroy");
        
        // trigger game object destroy timer
        Destroy(this.gameObject, this.destroyAnimationDuration);
    }
}
