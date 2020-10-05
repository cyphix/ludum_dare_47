using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class PlayerController : MonoBehaviour
{
    public float movementVelocity = 6.0f;
    public float movementJumpVelocity = 8.5f;
    public float jumpVelocity = 6.0f;
    public float wallJumpVelocityOut = 1.25f;
    public float wallJumpVelocityUp = 1.25f;
    public float wallSlideRate = 2.5f;
    public float maxJumpHeight = 4.0f;
    public float gravityScale = 2.0f;
    public float attackCooldown = 0.5f;
    public float attackMeleeTime = 0.30f;
    public float attackShootTime = 0.30f;
    public Transform groundCheckPoint;
    public Transform leftCheckPoint;
    public Transform rightCheckPoint;
    public LayerMask staticEnvironmentMask;
    public Timer timer;
    public GameObject bulletPrefab;
    public Text energyDisplay;
    public Text lifeDisplay;
    public IntersceneObject intersceneObject;
    public string endMenuScene;
    public bool turnOffWallJump = false;

    public int life = 100;
    public int energyLevels = 0; // out of 100
    public int temporalShiftNeeds = 90;

    private float moveLeftRight = 0.0f;
    private float jumpStartPos = 0.0f;
    private bool isGrounded = true;
    private bool isLeftWall = false;
    private bool isRightWall = false;
    private bool wallJump = false;
    private bool isJumpingPressed = false;
    private Rigidbody2D playerRigidBody;
    private Collider2D playerCollider;
    private float direction = 1.0f;
    private bool attackMelee = false;
    private bool attackShoot = false;
    private float attackCooldownLeft = 0.0f;
    private float attackAnimationCooldownLeft = 0.0f;

    private float temporalShiftButtonDownMax = 2.0f;
    private float temporalShiftButtonDownTime = 0.0f;

    private Animator animator;

    void Start()
    {
        this.playerRigidBody = GetComponent<Rigidbody2D>();
        this.playerCollider = GetComponent<Collider2D>();
        this.playerRigidBody.freezeRotation = true;
        this.playerRigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        this.playerRigidBody.gravityScale = gravityScale;

        this.animator = GetComponent<Animator>();
    }

    void Update()
    {
        // check for attacks
        /*if (Input.GetButtonDown("Fire1"))
        {
            if (this.attackCooldownLeft >= 0.0f && !this.attackMelee)
            {
                this.attackMelee = true;
                this.attackCooldownLeft = this.attackCooldown;
                this.attackAnimationCooldownLeft = this.attackMeleeTime;
            }
        }*/
        if(Input.GetButtonDown("Fire2"))
        {
            if (this.attackCooldownLeft >= 0.0f && !this.attackMelee)
            {
                this.attackShoot = true;
                this.attackCooldownLeft = this.attackCooldown;
                this.attackAnimationCooldownLeft = this.attackShootTime;
            }
        }

        // check for temporal shift use
        if (Input.GetKey(KeyCode.T))
        {
            this.temporalShiftButtonDownTime += Time.deltaTime;
            if (this.temporalShiftButtonDownTime >= this.temporalShiftButtonDownMax)
            {
                // use temporal shift
                this.temporalShiftButtonDownTime = 0.0f;
                this.UseTemporalShift();
            }
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            this.temporalShiftButtonDownTime = 0.0f;
        }

        // left/right move
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            this.moveLeftRight = Input.GetKey(KeyCode.A) ? -1 : 1;

            // determine direction and if the direction lock is on
            if(!Input.GetKey(KeyCode.LeftShift))
            {
                // no lock so turn character
                this.direction = Input.GetKey(KeyCode.A) ? -1 : 1;
            }
        }
        else
        {
            this.moveLeftRight = 0;
        }

        // jump
        if (Input.GetButtonDown("Jump") || Input.GetKey(KeyCode.W))
        {
            // check if the character is on the ground
            if (this.isGrounded)
            {
                this.isGrounded = false;
                this.isJumpingPressed = true;
                this.jumpStartPos = transform.position.y;
            }
            else
            {
                // if a wall jump(can't from ground)
                if (this.isLeftWall || this.isRightWall)
                {
                    this.wallJump = true;
                }
            }
        }
        if (Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W))
        {
            this.isJumpingPressed = false;
        }
    }

    void FixedUpdate()
    {
        //check for ground collision
        this.isGrounded = Physics2D.OverlapCircle(this.groundCheckPoint.position, 0.2f, this.staticEnvironmentMask);
        this.isLeftWall = Physics2D.OverlapCircle(this.leftCheckPoint.position, 0.1f, this.staticEnvironmentMask);
        this.isRightWall = Physics2D.OverlapCircle(this.rightCheckPoint.position, 0.1f, this.staticEnvironmentMask);

        //this.ClearLog();
        //Debug.Log("Bounds: " + this.playerCollider.bounds);

        if (this.attackMelee)
        {
            // attack
            this.animator.SetBool("attackMelee", true);

            this.attackMelee = false;
        }
        if (this.attackShoot)
        {
            // attack
            this.animator.SetBool("attackShoot", true);

            // create bullet
            GameObject bulletObject = Instantiate(bulletPrefab, gameObject.transform);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.direction = this.direction;
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
            if(
                (this.playerRigidBody.velocity.x > 0 && this.direction > 0) ||
                (this.playerRigidBody.velocity.x < 0 && this.direction < 0)
            )
            {
                // case of velocity in the same direct as character facing
                bulletRigidBody.velocity = new Vector2(this.playerRigidBody.velocity.x, 0.0f);
            }
            else
            {
                // opposite so flip the velocity
                bulletRigidBody.velocity = new Vector2(-this.playerRigidBody.velocity.x, 0.0f);
            }

            if (this.direction > 0)
            {
                bullet.transform.position += new Vector3(0.4f, 0.1f, 0.0f);
            }
            else
            {
                bullet.transform.position -= new Vector3(0.4f, -0.1f, 0.0f);
            }

            this.attackShoot = false;
        }
        else
        {
            // check if on wall
            if (!this.isGrounded)
            {
                if (!this.turnOffWallJump)
                {
                    if (this.wallJump)
                    {
                        if (this.isLeftWall && this.moveLeftRight == -1)
                        {
                            //this.playerRigidBody.velocity = new Vector2(this.wallJumpVelocityUp, this.wallJumpVelocityUp);
                            this.transform.position += new Vector3(this.wallJumpVelocityOut, this.wallJumpVelocityUp, 0.0f);
                            this.wallJump = false;
                            this.isJumpingPressed = true;
                            this.jumpStartPos = transform.position.y;
                        }
                        else if (this.isRightWall && this.moveLeftRight == 1)
                        {
                            //this.playerRigidBody.velocity = new Vector2(-this.wallJumpVelocityUp, this.wallJumpVelocityUp);
                            this.transform.position += new Vector3(-this.wallJumpVelocityOut, this.wallJumpVelocityUp, 0.0f);
                            this.wallJump = false;
                            this.isJumpingPressed = true;
                            this.jumpStartPos = transform.position.y;
                        }
                        else if (this.isLeftWall && this.moveLeftRight == 0)
                        {
                            this.transform.position += new Vector3(1.0f, 0.0f, 0.0f);
                            this.playerRigidBody.velocity = new Vector2(this.movementJumpVelocity, this.jumpVelocity);
                            this.wallJump = false;
                            this.isJumpingPressed = true;
                            this.jumpStartPos = transform.position.y;
                        }
                        else if (this.isRightWall && this.moveLeftRight == 0)
                        {
                            this.transform.position += new Vector3(-1.0f, 0.0f, 0.0f);
                            this.playerRigidBody.velocity = new Vector2(-this.movementJumpVelocity, this.jumpVelocity);
                            this.wallJump = false;
                            this.isJumpingPressed = true;
                            this.jumpStartPos = transform.position.y;
                        }
                    }
                    else
                    {
                        if (this.isLeftWall && this.moveLeftRight == -1)
                        {
                            this.playerRigidBody.velocity = new Vector2(this.playerRigidBody.velocity.x, -this.wallSlideRate);
                        }
                        else if (this.isRightWall && this.moveLeftRight == 1)
                        {
                            this.playerRigidBody.velocity = new Vector2(this.playerRigidBody.velocity.x, -this.wallSlideRate);
                        }
                        else
                        {
                            // left/right movement
                            this.playerRigidBody.velocity = new Vector2((this.moveLeftRight) * this.movementJumpVelocity, this.playerRigidBody.velocity.y);
                        }
                    }
                }
            }
            else
            {
                // set wall jump off
                this.wallJump = false;

                // left/right movement
                this.playerRigidBody.velocity = new Vector2((this.moveLeftRight) * this.movementVelocity, this.playerRigidBody.velocity.y);
            }

            // jump movement
            if (this.isJumpingPressed)
            {
                if ((transform.position.y - this.jumpStartPos) > this.maxJumpHeight)
                {
                    // since the max jump is reached set the jumping pressed to false regardless
                    this.isJumpingPressed = false;
                }
                this.playerRigidBody.velocity = new Vector2(this.playerRigidBody.velocity.x, this.jumpVelocity);
            }
        }

        // cooldowns
        if (this.attackCooldownLeft > 0)
        {
            this.attackCooldownLeft -= Time.deltaTime;
        }
        else
        {
            this.attackCooldownLeft = 0.0f;
        }
        if (this.attackAnimationCooldownLeft > 0)
        {
            this.attackAnimationCooldownLeft -= Time.deltaTime;
        }
        else
        {
            this.animator.SetBool("attackMelee", this.attackMelee);
            this.animator.SetBool("attackShoot", this.attackShoot);
            this.attackAnimationCooldownLeft = 0.0f;
        }

        // animator flags
        this.animator.SetBool("isGrounded", this.isGrounded);
        this.animator.SetFloat("moveSpeed", Mathf.Abs(this.playerRigidBody.velocity.x));
        this.animator.SetFloat("direction", this.direction);

        // life and energy displays
        this.energyDisplay.text = "Energy: " + this.energyLevels;
        this.lifeDisplay.text = "Life: " + this.life;
    }



    /*private void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }*/



    public void GiveEnergy(int amount)
    {
        if (this.energyLevels + amount > 100)
        {
            this.energyLevels = 100;
        }
        else
        {
            this.energyLevels += amount;
        }

        // play give energy animation
        // TODO: give energy animation
    }

    public void HurtPlayer(int amount, float direction)
    {
        if (this.life - amount > 0)
        {
            this.life -= amount;

            // play hurt animation
            // TODO: hurt animation
        }
        else
        {
            this.life = 0;

            this.TriggerDeath();
        }

        // knockback
        if(direction > 0)
        {
            // stop character and knockback to the right
            this.transform.position += new Vector3(2.0f, 0.3f, 0.0f);
            this.playerRigidBody.velocity = new Vector2(0.0f, 0.0f);
        }
        else
        {
            // stop character and knockback to the left
            this.transform.position += new Vector3(-2.0f, 0.3f, 0.0f);
            this.playerRigidBody.velocity = new Vector2(0.0f, 0.0f);
        }
    }

    public void TriggerDeath()
    {
        // play death animation
        // TODO: death animation
        
        this.intersceneObject.win = false;
        SceneManager.LoadScene(this.endMenuScene);
    }

    public void UseTemporalShift()
    {
        if (this.temporalShiftNeeds <= this.energyLevels)
        {
            this.energyLevels -= this.temporalShiftNeeds;
            this.timer.ResetTimer();
        }
        else
        {
            // play not enough energy
            // TODO: not enough energy animation
        }

        // play temporal shift animation
        // TODO: temporal shift animation
    }
}
