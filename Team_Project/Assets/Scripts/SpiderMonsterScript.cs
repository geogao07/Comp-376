using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMonsterScript : Enemy
{
    [SerializeField]
    private float sightRange;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float moveSpeed=1f;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private LineRenderer laserPreview;
    

    public bool facingRight = false;

    private bool lookingUp;
    
    private BoxCollider2D shieldCollider;
    Rigidbody2D myRigidBody;
    private float actionTimer;
    private GameObject player;
    private Transform bulletCenterObject;
    private Transform rigidCenter;
    private GameObject laser;
    private string state;
    private Animator animator;
    private float recoverTimer;
    private bool isAffectedByEnergyBlast;
    private Vector3 shootingDirection;

    private Vector3 knockedPosition;
    private bool knockLeft;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player/Iris");
        bulletCenterObject = transform.Find("Sprite/BulletCenter");
        rigidCenter = transform.Find("RigidCenter");
        shieldCollider = transform.Find("Sprite/FrontShield").GetComponent<BoxCollider2D>();    
        animator = gameObject.GetComponentInChildren<Animator>();
        isAffectedByEnergyBlast = false;
        state = "Idle";
        lookingUp = false;
        laserPreview = transform.Find("laserPreview").GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != "Dead") 
        { 
            
            if (state == "Move") Moving();
            else if (state == "Attack") Attacking();
            else if (state == "Attack2") Attacking2();
            else if (state == "Idle") Idling();

            if (actionTimer <= 0.0f) EvaluateAction();
            actionTimer -= Time.deltaTime;
            recoverTimer += Time.deltaTime;
        
        }
        UpdateAnimator();


    }

    public override void Death()
    {
        CancelAttack();

        state = "Dead";
        transform.gameObject.tag = "Corpse";
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }
       // this.GetComponent<Collider2D>().enabled = false;
        transform.gameObject.layer = LayerMask.NameToLayer("Corpse");
        shieldCollider.gameObject.tag = "Corpse";
        shieldCollider.gameObject.layer = LayerMask.NameToLayer("Corpse");
        shieldCollider.GetComponent<Collider2D>().enabled = false;
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        Destroy(laser);
    }    
    
    private void CheckAngle()
    {
        Vector3 direction = player.transform.position - transform.position;
        if (direction.y >= Mathf.Abs(direction.x)) lookingUp = true;
        else lookingUp = false;
        if (direction.x >= 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void Idling()
    {
        CheckAngle();
        return;
    }

    private void EvaluateAction()
    {
        

        float range = Mathf.Abs(bulletCenterObject.position.x - player.transform.position.x);
        if (!isAffectedByEnergyBlast)
        {
            if (range < attackRange) Attack();
            else if (range < sightRange) Move();
            else Idle();
        }
    }

    private void UpdateAnimator()
    {
        bool isWalking = false;
        bool isShooting = false;
        bool knockedLeft = false;
        bool knockedRight = false;
        bool isDead = false;


        if (state == "Attack" || state == "Attack2") isShooting = true;
        else if (state == "Move") isWalking = true;
        else if (state == "KnockedLeft") knockedLeft = true;
        else if (state == "KnockedRight") knockedRight = true;
        else if (state == "Dead") isDead = true;

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isShooting", isShooting);
        animator.SetBool("KnockedLeft", knockedLeft);
        animator.SetBool("KnockedRight", knockedRight);
        animator.SetBool("isDead", isDead);

        if (lookingUp)
        {
            animator.SetLayerWeight(0, 0.0f);
            animator.SetLayerWeight(1, 1.0f);
        }
        else
        {
            animator.SetLayerWeight(0, 1.0f);
            animator.SetLayerWeight(1, 0.0f);
        }
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.tag == "EnergyBlast")
    //    {
    //        recoverTimer = 0f;
    //        transform.RotateAround(rigidCenter.position, Vector3.forward, 180);
    //        isAffectedByEnergyBlast = true;

    //    }
    //}

    public override void ApplyEnergyBlast(Vector2 direction)
    {
        if (state == "KnockedLeft" || state == "KnockedRight") return;
        if (direction.y < 0.0f)
        {
            CancelAttack();
            state = "Idle";
            actionTimer = 1.0f;
            return;
        }

        CancelAttack();
        if (direction.x < 0.0f)
        {
            state = "KnockedLeft";
            animator.SetTrigger("KnockedLeft");
        }
        else
        {
            state = "KnockedRight";
            animator.SetTrigger("KnockedRight");
        }
        actionTimer = 4.0f;
    }    

    private void Idle()
    {
        actionTimer = 1.0f;
        state = "Idle";
    }

    private void Move()
    {
        if (!CanMove())
        {
            Attack();
            return;
        }
        state = "Move";
        actionTimer = 1.5f;
    }


    private bool CanMove()
    {
        float direction = player.transform.position.x - bulletCenterObject.position.x;
        Vector2 leftPoint = shieldCollider.bounds.center - shieldCollider.bounds.extents;
        Vector2 rightPoint = shieldCollider.bounds.center + shieldCollider.bounds.extents;
        rightPoint = new Vector2(rightPoint.x, leftPoint.y);

        Vector2 checkPoint = leftPoint;
       
        if (direction > 0.0f) checkPoint = rightPoint;
        checkPoint.y += 0.1f;
        Debug.Log(checkPoint);

        if (!Physics2D.Raycast(checkPoint, Vector2.down, 0.3f, groundLayer))
        {
            Debug.Log("downward fail.");
            return false;
        }
        direction = direction / Mathf.Abs(direction);
        if (Physics2D.Raycast(checkPoint, new Vector2(direction, 0.0f), 0.3f, groundLayer))
        {
            Debug.Log("side fail");
            return false;
        }

        return true;
    }
    private void Moving()
    {
       
         float direction = player.transform.position.x - bulletCenterObject.position.x;
         direction = direction / Mathf.Abs(direction) * moveSpeed;
         myRigidBody.MovePosition(new Vector2(transform.position.x + direction * Time.deltaTime, transform.position.y));
    }

    //private void CountDownAndShoot()
    //{
    //    //shotCounter -= Time.deltaTime;
    //    //if (shotCounter <= 0f)
    //    //{
    //        Fire();
    //    //    shotCounter = UnityEngine.Random.Range(0.5f, 5f);

    //    //}
        

    //}

    private void CancelAttack()
    {
        laserPreview.enabled = false;
        laserPreview.gameObject.SetActive(false);
    }

    private void Attack()
    {
        
        actionTimer = 3.0f;
        state = "Attack";
        CheckAngle();
    }

    private void Attacking()
    {
        CheckAngle();
        if (actionTimer <= 2.0f) AttackPhase2();
    }

    public void AttackPhase2()
    {
        state = "Attack2";

        shootingDirection = player.transform.position - bulletCenterObject.transform.position;

        Attacking2();
    }
    
    public void Attacking2()
    {
        laserPreview.enabled = true;
        laserPreview.gameObject.SetActive(true);
        Vector3 lastPoint = bulletCenterObject.transform.position;
        lastPoint.z = 0;
        int vertexCounter = 1;
        laserPreview.SetVertexCount(1);
        laserPreview.SetPosition(0, lastPoint);
        bool loop = true;
        Vector3 tempShootingDirection = shootingDirection;


        while (loop)
        {
            RaycastHit2D hit = Physics2D.Raycast(lastPoint, tempShootingDirection, 30f, 1 << LayerMask.NameToLayer("IceWall"));


            if (hit.collider != null)
            {
                vertexCounter += 1;
                laserPreview.SetVertexCount(vertexCounter);


                laserPreview.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastPoint, 0.01f));
                //laserPreview.SetPosition(vertexCounter - 1, hit.point);
                lastPoint = hit.point;
                lastPoint.z = 0;
                tempShootingDirection = Vector3.Reflect(tempShootingDirection, hit.normal);
            }
            else
            {
                vertexCounter++;
                laserPreview.SetVertexCount(vertexCounter);
                laserPreview.SetPosition(vertexCounter - 1, lastPoint + (tempShootingDirection.normalized * 100));
                loop = false;
            }
        }

        if (actionTimer <= 1.0f)
        {
            laserPreview.enabled = false;
            laserPreview.gameObject.SetActive(false);
            GameObject laserObject = Instantiate(laserPrefab, bulletCenterObject.transform.position, Quaternion.identity) as GameObject;
            Laser laser = laserObject.GetComponent<Laser>();
            laser.tag = "Laser-Enemy";
            laser.setDirection(shootingDirection);
            state = "Idle";
        }
    }
}

