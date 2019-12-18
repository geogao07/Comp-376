using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMonster : Enemy
{

    [SerializeField]
    private float sightRange;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float patrolRange;
    [SerializeField]
    private LayerMask groundLayer; // Holds reference to Ground layer, for raycast purpose.

    Rigidbody2D myRigidBody;
    private BoxCollider2D collider;
    private GameObject player;
    private string state;
    private string nextState;
    private Animator animator;
    Vector3 startPos;
    private bool canAttack;
    private bool dirLeft= true;
    private float actionTimer;
    private float attackCD;
    private bool isIdle=false;
    private Vector3 currentPlayerPosition;


    // Attributes used for the attack trajectory.
    private Vector3 attackPosition;
    private float attackDistance;
    private float diveDistance;
    private float attackDuration;

    // For animator purpose.
    private Vector3 lastPos;
    private Vector3 velocity;
    
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player/Iris");
        animator = gameObject.GetComponent<Animator>();
        state = "Idle";
        nextState = "Idle";
        startPos = gameObject.transform.position;
        canAttack = true;

        actionTimer = 0.0f;
        
        attackPosition = new Vector3(0.0f, 0.0f, 0.0f);
        attackDistance = 0.0f;
        diveDistance = 0.0f;
        attackDuration = 0.0f;

        lastPos = transform.position;
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != "Dead")
        {

            if (state == "Idle") Idling();
            else if (state == "Attack") Attacking();
            else if (state == "Hurt") CheckGrounded();
            else if (state == "FlyBack") FlyingBack();

            if (actionTimer <= 0.0f) EvaluateAction();
            actionTimer -= Time.deltaTime;
            if (attackCD > 0.0f) attackCD -= Time.deltaTime;

            
        }

        updateAnimator();

    }

    public override void Death()
    {

        state = "Hurt";
        nextState = "Dead";
        transform.gameObject.tag = "Corpse";
        this.GetComponent<Collider2D>().enabled = false;
        transform.gameObject.layer = LayerMask.NameToLayer("Corpse");
        myRigidBody.velocity = velocity;
        myRigidBody.gravityScale = 1f;
            
        
    }

    private void Idle()
    {
        state = "Idle";
        actionTimer = 0.1f;
        return;
    }

    private void Idling()
    {
        if (transform.position.x > startPos.x + patrolRange)
        {
            dirLeft = true;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (transform.position.x < startPos.x - patrolRange)
        {
            dirLeft = false;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

        Vector3 tempPos = transform.position;

        if (dirLeft)
        {
            myRigidBody.MovePosition(new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y));
            //if (transform.position == tempPos)
            //{
            //    dirLeft = false;
            //    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            //}
        }
        else
        {
            myRigidBody.MovePosition(new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y));
            //if (transform.position == tempPos)
            //{
            //    dirLeft = true;
            //    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //}
        }

    }

    private void updateAnimator()
    {
        Vector3 tempVel = (transform.position - lastPos) / Time.deltaTime;
        if (tempVel != Vector3.zero) velocity = tempVel;
        lastPos = transform.position;

        bool isDead = false;
        bool isHurt = false;
        bool isKnocked = false;
        
        if (state == "Dead") isDead = true;
        else if (state == "Hurt") isHurt = true;
        else if (state == "Knocked") isKnocked = true;
        
        animator.SetBool("isDead", isDead);
        animator.SetBool("isHurt", isHurt);
        animator.SetBool("isKnocked", isKnocked);

        animator.SetFloat("verticalVelocity", velocity.y);

    }

    private void EvaluateAction()
    {
        if (state == "Attack")
        {
            attackCD = 2.0f;
            startPos = transform.position;
        }
        else if (state == "Knocked")
        {
            FlyBack();
            return;
        }

        float range = (player.transform.position - transform.position).magnitude;
        if (attackCD <= 0.0f && range <= attackRange && transform.position.y > player.transform.position.y) Attack();
        else Idle();
    }

    private void FlyBack()
    {
        state = "FlyBack";
        transform.Translate(0.0f, 0.1f, 0.0f);
        myRigidBody.gravityScale = 0.0f;
        if ((startPos - transform.position).x >= 0.0f)
        {
            dirLeft = false;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            dirLeft = true;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        FlyingBack();
    }

    private void FlyingBack()
    {
        actionTimer = 5.0f;

        myRigidBody.MovePosition(transform.position + (startPos - transform.position).normalized * moveSpeed * 1.5f * Time.deltaTime);
        

        if ((startPos - transform.position).magnitude <= 0.1f)
        {
            transform.position = startPos;
            EvaluateAction();
        }
    }
    
    private void Attack()
    {
        if (state == "Attack")
        {
            Idle();
            return;
        }
        attackPosition = transform.position;
        attackDistance = (player.transform.position.x - transform.position.x) * 2.0f;
        diveDistance = (player.transform.position.y - transform.position.y) / 2.0f;
        actionTimer = (player.transform.position - transform.position).magnitude / attackSpeed;
        attackDuration = actionTimer;
        state = "Attack";
        FacingPlayer();

    }
    private void Attacking()
    {
        float attackProgress = 1.0f - actionTimer / attackDuration;
        myRigidBody.MovePosition(attackPosition + new Vector3(attackDistance * attackProgress, diveDistance * (1.0f - Mathf.Cos(Mathf.PI * 2.0f * attackProgress))));
    }
    
    private void FacingPlayer()
    {
        float direction = player.transform.position.x - myRigidBody.position.x;
        if (direction >= 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void CheckGrounded()
    {
        Vector2 leftPoint = collider.bounds.center - collider.bounds.extents;
        Vector2 rightPoint = collider.bounds.center + collider.bounds.extents;

        //rightPoint = new Vector2(rightPoint.x, leftPoint.y);
        //leftPoint = new Vector2(Mathf.Lerp(leftPoint.x, rightPoint.x, 0.02f), leftPoint.y);
        //rightPoint = new Vector2(Mathf.Lerp(leftPoint.x, rightPoint.x, 0.98f), rightPoint.y);


        if ((Physics2D.Raycast(leftPoint, Vector2.down, 0.1f, groundLayer) || Physics2D.Raycast(rightPoint, Vector2.down, 0.1f, groundLayer))
            && Mathf.Abs(myRigidBody.velocity.y) < 0.1f)
        {

            myRigidBody.velocity = Vector2.zero;
            if (nextState == "Knocked")
            {
                state = "Knocked";
                actionTimer = 0.7f;
            }
            else if (nextState == "Dead") state = "Dead";
        }
        else
        {

            actionTimer = 0.5f;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "IceWall")
        {
           
            GameObject[] iceWall = GameObject.FindGameObjectsWithTag("IceWall");

            foreach (GameObject icewall in iceWall)
            {
                Destroy(icewall);
            }
        }

    }

    public override void ApplyEnergyBlast(Vector2 direction)
    {
        state = "Hurt";
        nextState = "Knocked";
        myRigidBody.velocity = direction.normalized * 10.0f;
        myRigidBody.gravityScale = 2.0f;
    }

}
