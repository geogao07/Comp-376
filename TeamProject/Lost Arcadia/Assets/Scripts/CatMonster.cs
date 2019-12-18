using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMonster : Enemy
{
    
    [SerializeField] float speed;
    [SerializeField] int damage = 2;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate;
    [SerializeField] float fireRange;
    [SerializeField] float sightRange;
    [SerializeField] float meleeRange;
    [SerializeField] private LayerMask groundLayer; // Holds reference to Ground layer, for raycast purpose.
    [SerializeField] private LayerMask irisLayer; // Holds reference to Iris layer, for raycast purpose.
    private float actionTimer;
    private string state;
    private string nextState;
    private float direction;
    private int currentBullet;

    private Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    Transform[] firePositions;

    private bool hasEntered;
    float prevDist;
    private  float lastFire;
    private bool IrisClose = false;
    private Vector3 FiringDirection = new Vector3 (-1,0,0);
    private GameObject iris;

    // Start is called before the first frame update
    void Start()
    {
        actionTimer = 0.0f;
        state = "Idle";
        nextState = "";
        currentBullet = 0;

        lastFire = Time.time;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        anim = animator;
        iris = GameObject.Find("Player/Iris");
        firePositions = new Transform[3];
        firePositions[0] = transform.Find("firing1");
        firePositions[1] = transform.Find("firing2");
        firePositions[2] = transform.Find("firing3");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(state);
        if (state != "Dead")
        {

            if (state == "Move") Moving();
            else if (state == "Idle") Idling();
            else if (state == "Jump" || state == "Hurt") CheckGrounded();

            if (actionTimer <= 0.0f) EvaluateAction();
            actionTimer -= Time.deltaTime;
        }
        UpdateAnimator();
    }

    private void EvaluateAction()
    {
        if (state == "Knocked")
        {
            Idle();
            actionTimer *= 0.5f;
            return;
        }
        float range = Mathf.Abs(transform.position.x - iris.transform.position.x);
        if (range < fireRange) Attack();
        else if (range < sightRange) Move();
        else Idle();

    }


    private void Attack()
    {
        if (CanMove() == 4)
        {
            Move();
            return;
        }
        
        actionTimer = 3.0f;
        state = "Attack";
        currentBullet = 0;
    }

    public void Shoot()
    {
        GameObject temp = Instantiate(bulletPrefab, firePositions[currentBullet].position, Quaternion.identity);
        temp.GetComponent<Bullets>().setDirection(iris.transform.position - temp.transform.position);
        currentBullet++;
        if (currentBullet >= 3)
        {
            state = "Idle";
            currentBullet = 0;
        }
    }

    private void Move()
    {
        int action = CanMove();
        FacePlayer();
        if (action == 1) // Move back
        {
            direction *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1.0f, 1.0f, 1.0f);
            state = "Move";
            actionTimer = 0.1f;
            return;
        }
        else if (action == 2) // Jump Low
        {
            Jump(new Vector2(direction * 0.6f, 3.0f), "Move");
        }
        else if (action == 3) // Jump High
        {
            Jump(new Vector2(direction * 0.3f, 10.0f), "Move");
        }
        else if (action == 4) // Jump Forward
        {
            Jump(new Vector2(direction, 5.0f), "Idle");
        }
        else
        {
            state = "Move";
            actionTimer = 0.1f;
        }
        
    }

    // Returns a code to determine action. 0 = Move, 1 = Move back, 2 = Jump Low, 3 = Jump High, 4 = Jump toward Iris.
    private int CanMove()
    {
        direction = iris.transform.position.x - transform.position.x;
        direction = direction / Mathf.Abs(direction) * speed;
        Vector2 leftPoint = collider.bounds.center - collider.bounds.extents;
        Vector2 rightPoint = collider.bounds.center + collider.bounds.extents;
        rightPoint = new Vector2(rightPoint.x, leftPoint.y);

        Vector2 checkPoint = leftPoint;
        if (direction > 0.0f) checkPoint = rightPoint;
        checkPoint.y += 0.1f;

        // Check case 1: Too close to wall
        if (Physics2D.Raycast(checkPoint, new Vector2(direction, 0.0f), 0.2f, groundLayer))
        {
            
            return 1;
        }

        // Check case 2: End of ledge
        if (!Physics2D.Raycast(checkPoint, Vector2.down, 0.3f, groundLayer))
        {
            
            return 2;
        }

        // Check case 3: Incoming wall
        if (Physics2D.Raycast(checkPoint, new Vector2(direction, 0.0f), 1.0f, groundLayer))
        {
            
            return 3;
        }

        // Check case 4: Iris is close
        if (Physics2D.Raycast(checkPoint, new Vector2(direction, 0.0f), meleeRange, irisLayer))
        {
            
            return 4;
        }

        return 0;
    }

    private void Jump(Vector2 velocity, string endState)
    {
        
        rb.velocity = velocity;
        //rb.MovePosition(new Vector2(transform.position.x, transform.position.x + velocity.y * Time.deltaTime));
        state = "Jump";
        nextState = endState;
    }

    private void CheckGrounded()
    {
        Vector2 leftPoint = collider.bounds.center - collider.bounds.extents;
        Vector2 rightPoint = collider.bounds.center + collider.bounds.extents;

        //rightPoint = new Vector2(rightPoint.x, leftPoint.y);
        //leftPoint = new Vector2(Mathf.Lerp(leftPoint.x, rightPoint.x, 0.02f), leftPoint.y);
        //rightPoint = new Vector2(Mathf.Lerp(leftPoint.x, rightPoint.x, 0.98f), rightPoint.y);


        if ((Physics2D.Raycast(leftPoint, Vector2.down, 0.1f, groundLayer) || Physics2D.Raycast(rightPoint, Vector2.down, 0.1f, groundLayer))
            && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            
            rb.velocity = Vector2.zero;
            if (nextState == "Idle") Idle();
            else if (nextState == "Move") Move();
            else if (nextState == "Knocked")
            {
                state = "Knocked";
                actionTimer = 0.4f;
            }
            else if (nextState == "Dead") state = "Dead";
        }
        else
        {
            
            actionTimer = 0.5f;
        }
    }

    private void Moving()
    {
        rb.MovePosition(new Vector2(transform.position.x + direction * Time.deltaTime, transform.position.y));
    }

    private void Idle()
    {
        actionTimer = 1.0f;
        state = "Idle";
    }

    private void Idling()
    {
        FacePlayer();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<IrisController>().Damage(damage, col.transform.position - transform.position);

            //player takeDamage
        }
    }

    private void FacePlayer()
    {
        if (iris.transform.position.x - transform.position.x >= 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public override void Death()
    {
        state = "Hurt";
        nextState = "Dead";
        CheckGrounded();
        gameObject.tag = "Corpse";
        gameObject.layer = LayerMask.NameToLayer("Corpse");
        rb.drag = 5.0f;
        rb.gravityScale *= 2f;
    }

    private void UpdateAnimator()
    {
        bool isMoving = false;
        bool isFiring = false;
        bool isDead = false;
        bool isHurt = false;
        bool isKnocked = false;

        if (state == "Attack") isFiring = true;
        else if (state == "Move") isMoving = true;
        else if (state == "Dead") isDead = true;
        else if (state == "Hurt") isHurt = true;
        else if (state == "Knocked") isKnocked = true;

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isFiring", isFiring);
        animator.SetBool("isDead", isDead);
        animator.SetBool("isHurt", isHurt);
        animator.SetBool("isKnocked", isKnocked);
        animator.SetFloat("verticalVelocity", rb.velocity.y);
    }

    public override void ApplyEnergyBlast(Vector2 direction)
    {
        state = "Hurt";
        nextState = "Knocked";
        rb.velocity = direction.normalized * 5.0f;
    }
}