using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IrisController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed; // How fast the character walks.
    [SerializeField]
    private float dashSpeed; // How fast the character dashes.
    [SerializeField]
    private float jumpSpeed; // How fast the character jumps while holding Up.
    [SerializeField]
    private float jumpTime; // How long the Up button can be held to increase height.
    [SerializeField]
    private float dashTriggerTime; // Time between two Left/Right key pushes to start dashing.
    [SerializeField]
    private float distanceToGround; // Threshold for the Raycast to consider the character grounded.
    [SerializeField]
    private float exitBattleTime; // Time to return to normal stance after defeating the last nearby enemy.
    [SerializeField]
    private float skillAnimationTime; // Time that Iris will look like she is using a skill.
    [SerializeField]
    private float hurtTime; // Time that Iris will be unable to move after getting hit.
    [SerializeField]
    private float invincibleTime; // Time that Iris will be invincible after getting hit.
    [SerializeField]
    private float baseKnockback; // How much knockback Iris takes for 1 damage.
    [SerializeField]
    private float blastDuration; // How much time Iris can't move after using Energy Blast on herself.

    [SerializeField]
    private LayerMask groundLayer; // Holds reference to Ground layer, for raycast purpose.



    //add some audio sources
    public AudioSource injury;
    public AudioSource jump;
    public AudioSource land;
   
    

    // Other components of the character.
    private Animator animator;
    private Rigidbody2D rigidBody;
    private EdgeCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private HealthBarController healthBar;
    private Transform deathResumePoint;

    private bool isGrounded; 
    private bool isDashing;
    private bool isCrouching;
    private bool isFloating; // Inside Gravitational Field
    private bool isJumping; // Gaining height by actively holding the Jump button. 

    private float dashInput; // Time delay remaining between two key presses to trigger a dash.
    private float jumpInput; // Time remaining that the player can hold the jump button to gain height.
    private float horizontalVelocity; // How much Iris is currently moving horizontally. Saved for Hurt and Animator uses.
    private float battleTime; // For animator use. When Standing still, Iris is in fighting pose while this is > 0.0f.
    private float skillTime; // For animator use. Iris is shown using a skill while this is > 0.0f.
    private float xVector; // For animator use. Mouse X position, determining where Iris is looking at.
    private float remainingInvincibleTime; // Time counted down after getting hit.
    private float baseGravity; // Saved for when vertical velocity is disabled.
    private float baseDrag; // Saved for Energy Blast.
    private float remainingBlastTime; // How much time remaining until Iris gets affected by normal gravity again
    private float keepMomentumTime; // How much time after getting hit by Energy Blast where pressing no key makes Iris keep her momentum

    private string lastDirection; // Last horizontal direction key pressed. Used to control dashing.

    public static IrisController iris;
    public bool onPlat = false;

    public static bool canMove; //to stop moving in some situations
    // Start is called before the first frame update
    public static bool hasCard = false; // to check card before entring the lab
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<EdgeCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar = GetComponent<HealthBarController>();

        isGrounded = true;
        isDashing = false;
        isCrouching = false;
        isFloating = false;
        isJumping = false;

        dashInput = 0.0f;
        jumpInput = 0.0f;
        horizontalVelocity = 0.0f;
        battleTime = exitBattleTime;
        skillTime = 0.0f;
        xVector = 1.0f;
        remainingInvincibleTime = 0.0f;
        baseGravity = rigidBody.gravityScale;
        baseDrag = rigidBody.drag;
        remainingBlastTime = 0.0f;

        lastDirection = "";

        iris = this;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //DebugFunctions();


        //Debug.Log(rigidBody.velocity);

        if (CheckHurt())
        {
            CheckGrounded();
            CheckBattle();
            Crouch();
            if (remainingBlastTime < blastDuration / 2.0f) HorizontalMove();
            if (remainingBlastTime <= 0.0f) VerticalMove();
            else BlastMove();
            CheckSkill();
        }

        CheckDead();

        UpdateAnimator();
    }

    private void CheckDead()
    {
        if (healthBar.isDead())
        {
            GameObject[] resumePoints = GameObject.FindGameObjectsWithTag("ResumePoint");
            int index = -1;
            float minDistance = float.MaxValue;

            for (int i = 0; i < resumePoints.Length; i++)
            {
                float currentDistance = Vector3.Distance(transform.position, resumePoints[i].transform.position);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    index = i;
                }
            }

            if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                SceneManager.LoadScene(5);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 8)
            {
                SceneManager.LoadScene(8);
            }

            transform.position = resumePoints[index].transform.position;
            healthBar.reset();
        }
    }

    // Hard-coded functions to test specific functions.
    private void DebugFunctions()
    {
        if (canMove && Input.GetKeyDown(KeyCode.H))
        {
            Damage(1, transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition) );
        }
        else if (canMove && Input.GetKeyDown(KeyCode.J))
        {
            Damage(3, transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (canMove && Input.GetKeyDown(KeyCode.Q))
        {
            ApplyEnergyBlast(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    // Manages Iris after she takes damage. If she is able to move, the function returns true. Else, the function will take care of how she moves.
    private bool CheckHurt()
    {
        if (remainingInvincibleTime <= 0.0f) return true;
        if (remainingInvincibleTime > 0.0f) remainingInvincibleTime -= Time.deltaTime; 
        if (remainingInvincibleTime > invincibleTime - hurtTime) // Case: Iris cannot move.
        {
            
            horizontalVelocity = Mathf.Lerp(Mathf.Abs(horizontalVelocity), baseKnockback, 4.0f * Time.deltaTime);
            if (lastDirection == "Right") horizontalVelocity *= -1;
            rigidBody.velocity = new Vector2(horizontalVelocity, 0.0f);
            rigidBody.gravityScale = 0.0f;
            return false;
        }
        else
        {
            if (remainingInvincibleTime > 0.0f) spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            else spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            rigidBody.gravityScale = baseGravity;
            return true;
        }
    }

    // Sends a small raytrace from the BoxCollider's bottom corner, and updates isGrounded accordingly.
    private void CheckGrounded()
    {
        Vector2 leftPoint = collider.points[3] + new Vector2(transform.position.x, transform.position.y);
        Vector2 rightPoint = collider.points[4] + new Vector2(transform.position.x, transform.position.y);

        //rightPoint = new Vector2(rightPoint.x, leftPoint.y);
        //leftPoint = new Vector2(Mathf.Lerp(leftPoint.x, rightPoint.x, 0.02f), leftPoint.y);
        //rightPoint = new Vector2(Mathf.Lerp(leftPoint.x, rightPoint.x, 0.98f), rightPoint.y);

        if (onPlat)
        {
            isGrounded = true;
            
        }
        else if ((Physics2D.Raycast(leftPoint, Vector2.down, distanceToGround, groundLayer) || Physics2D.Raycast(rightPoint, Vector2.down, distanceToGround, groundLayer))
            && Mathf.Abs(rigidBody.velocity.y) < 0.2f)
        {
            isGrounded = true;
            
        }
        else
        {
            isGrounded = false;
        }
    }

    private void CheckBattle()
    {
        if (battleTime > 0.0f) battleTime -= Time.deltaTime;
    }

    // Manages horizontal input and movement.
    private void HorizontalMove()
    {
        if (keepMomentumTime > 0.0f) keepMomentumTime -= Time.deltaTime;
        CheckDash();
        float speed = walkSpeed;
        horizontalVelocity = 0.0f;
        if (!isGrounded && keepMomentumTime > 0.0f) horizontalVelocity = rigidBody.velocity.x;
        else if (!isGrounded) horizontalVelocity = Mathf.Lerp(rigidBody.velocity.x, 0.0f, 15.0f * Time.deltaTime);
        if (isDashing) speed = dashSpeed;
        if (isCrouching) speed = 0.0f;
        if (canMove && Input.GetButtonDown("Left")) lastDirection = "Left";
        else if (canMove && Input.GetButtonDown("Right")) lastDirection = "Right";
        if (canMove && Input.GetButton("Left") && (lastDirection == "Left" || !Input.GetButton("Right"))) 
        {
            //transform.Translate(Vector2.left * speed * Time.deltaTime);
            horizontalVelocity = -speed;
            FlipDirection(true);
            lastDirection = "Left";
        }
        if (canMove && Input.GetButton("Right") && (lastDirection == "Right" || !Input.GetButton("Left")))
        {
            //transform.Translate(Vector2.right * speed * Time.deltaTime);
            horizontalVelocity = speed;
            FlipDirection(false);
            lastDirection = "Right";
        }
        rigidBody.velocity = new Vector2(horizontalVelocity, rigidBody.velocity.y);
    }
    
    private void Crouch()
    {
        isCrouching = isGrounded && Input.GetButton("Down") && !isDashing;
    }

    // Checks for input to determine if the character is dashing or not.
    // The player must double-tap a direction to start dashing, and continues dashing until releasing the key.
    // Dashing does not get interrupted when releasing in mid-air.
    private void CheckDash()
    {
        if (isDashing)
        {
            if (isGrounded && (!Input.GetButton(lastDirection) || (Input.GetButton("Left") && Input.GetButton("Right"))))
            {
                isDashing = false;
            }
            
            
        }
        else
        {
            if (canMove && Input.GetButtonDown("Left"))
            {
                if (lastDirection == "Left" && dashInput > 0.0f && isGrounded) isDashing = true;
                else
                {
                    //lastDirection = "Left";
                    dashInput = dashTriggerTime;
                }
            }
            else if (canMove && Input.GetButtonDown("Right"))
            {
                if (lastDirection == "Right" && dashInput > 0.0f && isGrounded) isDashing = true;
                else
                {
                    //lastDirection = "Right";
                    dashInput = dashTriggerTime;
                }
            }
            if (dashInput > 0.0f) dashInput -= Time.deltaTime;
        }
    }

    // Manages vertical movement, or more specifically, jumping. 
    private void VerticalMove()
    {
        // When jumping, the character keeps the same velocity until reaching a certain height (calculated in time and speed) or until releasing the jump button.
        if (isJumping) // Case: currently controlling jump height
        {
            
            if (jumpInput > 0.0f && Input.GetButton("Up")) // Case: Continue increasing height
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
                jumpInput -= Time.deltaTime;
            }
            else // Case: Stop increasing height and start falling.
            {
                jumpInput = 0.0f;
                isJumping = false;
            }


        }
        else if (isGrounded && Input.GetButtonDown("Up")) // Case: Initiating a jump.
        {
            jump.Play();
            rigidBody.velocity = Vector2.up * jumpSpeed;
            isJumping = true;
            jumpInput = jumpTime;
        }
        
    }

    // Manages how Iris moves after being hit by her own Energy Blast.
    private void BlastMove()
    {
        remainingBlastTime -= Time.deltaTime;

        if (isGrounded || remainingBlastTime <= 0.0f)
        {
            rigidBody.gravityScale = baseGravity;
            rigidBody.drag = baseDrag;
        }
        else
        {
            rigidBody.gravityScale = Mathf.Lerp(rigidBody.gravityScale, baseGravity, 1.0f * Time.deltaTime);
            rigidBody.drag = Mathf.Lerp(rigidBody.drag, baseDrag, 1.0f * Time.deltaTime);
        }
    }

    // Cools down the skill animation.
    private void CheckSkill()
    {
        if (skillTime > 0.0f) skillTime -= Time.deltaTime;
    }

    // Make Iris use her skill animation.
    public void UseSkill()
    {
        skillTime = skillAnimationTime;
    }

    // Send all the necessary data for the Animator to update.
    private void UpdateAnimator()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("horizontalVelocity", Mathf.Abs(horizontalVelocity) / dashSpeed);
        animator.SetFloat("verticalVelocity", rigidBody.velocity.y);
        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isBattling", battleTime > 0.0f);
        animator.SetBool("isHurt", remainingInvincibleTime > invincibleTime - hurtTime);

        float[] layerWeights = { 0.0f, 0.0f, 0.0f, 0.0f };
        if (IsLookingBackward())
        {
            layerWeights[1] += 1.0f;
            layerWeights[3] += 1.0f;
        } else
        {
            layerWeights[0] += 1.0f;
            layerWeights[2] += 1.0f;
        }
        if (skillTime > 0.0f)
        {
            layerWeights[2] += 1.0f;
            layerWeights[3] += 1.0f;
        } else
        {
            layerWeights[0] += 1.0f;
            layerWeights[1] += 1.0f;
        }


        for (int i = 0; i < 4; i++)
        {
            layerWeights[i] = Mathf.Max(0.0f, layerWeights[i] - 1.0f);
            animator.SetLayerWeight(i, layerWeights[i]);
        }
    }

    // Changes the direction Iris' body is facing toward
    private void FlipDirection(bool facingLeft)
    {
        //spriteRenderer.flipX = facingLeft;
        if (facingLeft) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    // Returns a boolean based on the direction Iris' head is looking at. The direction she looks at is locked while using a skill.
    private bool IsLookingBackward()
    {
        if (skillTime <= 0.0f) xVector = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        if (lastDirection == "Left")
        {
            return xVector > 0.0f;
        }
        else
        {
            return xVector < 0.0f;
        }
    }

    // Inflict damage on Iris. Power is damage taken, and direction is the vector leading from the damage source to Iris' position.
    public void Damage(int power, Vector3 direction)
    {
        if (remainingInvincibleTime <= 0.0f)
        {
            injury.Play();
            gameObject.AddComponent<DamageFlash>();
            battleTime = exitBattleTime;
            horizontalVelocity = baseKnockback * Mathf.Pow(power, 1.5f);
            remainingInvincibleTime = invincibleTime;
            healthBar.updateHP(power);
            if (direction.x > 0.0f)
            {
                lastDirection = "Left";
                FlipDirection(true);
            }
            else
            {
                lastDirection = "Right";
                FlipDirection(false);
                horizontalVelocity *= -1;
            }
        }
    }

    // Called when Iris enters the area of effect from her own Energy Blast. Sets up all the variables needed to make it working.
    public void ApplyEnergyBlast(Vector2 direction)
    {
        if (direction.Equals(Vector2.zero)) direction = Vector2.up;
        direction.y *= 3.0f;
        direction = direction.normalized;
        
        direction.x *= dashSpeed;
        direction.y *= jumpSpeed * 0.8f;
        rigidBody.velocity = direction;
        if (direction.x < 0.0f)
        {
            lastDirection = "Left";
            FlipDirection(true);
        }
        else
        {
            lastDirection = "Right";
            FlipDirection(false);
        }
        isDashing = true;
        remainingBlastTime = blastDuration;
        keepMomentumTime = blastDuration + 0.3f;
        rigidBody.gravityScale = 0.0f;
        rigidBody.drag = 0.0f;
    }

}
