using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiedBeetle : Enemy
{
    [SerializeField]
    private float sightRange;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float bulletSize;
    [SerializeField]
    private LayerMask groundLayer; // Holds reference to Ground layer, for raycast purpose.
    [SerializeField] GameObject eggPrefab;
    [SerializeField] bool shootsEggs = false;
    private float actionTimer;
    private string state;

    private GameObject player;
    private Transform spriteObject;
    private BoxCollider2D shieldCollider;
    private Transform bulletCenterObject;
    private GameObject chargingBullet;
    private GameObject catEgg;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private int attacks = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player/Iris");
        spriteObject = transform.Find("BeetleSprite");
        shieldCollider = transform.Find("Shield").GetComponent<BoxCollider2D>();
        bulletCenterObject = transform.Find("BulletCenter");
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        state = "Idle";
        canKnockBack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != "Dead")
        {

            if (state == "Move") Moving();
            else if (state == "Attack") Attacking();
            else if (state == "EggAttack") EggAttack();
            else if (state == "Idle") Idling();

            if (actionTimer <= 0.0f) EvaluateAction();
            actionTimer -= Time.deltaTime;
        }
        UpdateAnimator();
    }

    private void EvaluateAction()
    {
        float range = Mathf.Abs(bulletCenterObject.position.x - player.transform.position.x);
        if (range < attackRange) Attack();
        else if (range < sightRange) Move();
        else Idle();

    }

    private void Attack()
    {
        if (attacks < 2)
        {
            chargingBullet = Instantiate(bulletPrefab, bulletCenterObject.position, Quaternion.identity);
            chargingBullet.GetComponent<Rigidbody2D>().simulated = false;
            actionTimer = 3.0f;
            if(shootsEggs) attacks++;
            state = "Attack";
        }
        else
        {
            attacks = 0;
            catEgg = Instantiate(eggPrefab, bulletCenterObject.position, Quaternion.identity);
            catEgg.GetComponent<Rigidbody2D>().simulated = false;
            actionTimer = 2.0f;
            state = "EggAttack";
        }

    }

    private void Attacking()
    {
        chargingBullet.transform.localScale = Vector3.Lerp(chargingBullet.transform.localScale, new Vector3(bulletSize, bulletSize, bulletSize), 1.0f * Time.deltaTime);
        if (actionTimer < 1.0f)
        {
            chargingBullet.GetComponent<Bullets>().setDirection(player.transform.position - chargingBullet.transform.position);
            chargingBullet.GetComponent<Rigidbody2D>().simulated = true;
            state = "Idle";
        }
    }

    private void EggAttack()
    {

        catEgg.GetComponent<Rigidbody2D>().simulated = true;
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
        rigidBody.MovePosition(new Vector2(transform.position.x + direction * Time.deltaTime, transform.position.y));
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

    private void FacePlayer()
    {
        float direction = player.transform.position.x - bulletCenterObject.position.x;
        if (direction >= 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void UpdateAnimator()
    {
        bool isWalking = false;
        bool isShooting = false;
        bool isDead = false;

        if (state == "Attack") isShooting = true;
        else if (state == "Move") isWalking = true;
        else if (state == "Dead") isDead = true;

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isShooting", isShooting);
        animator.SetBool("isDead", isDead);
    }

    public override void Death()
    {
        state = "Dead";
        GetComponent<CircleCollider2D>().enabled = false;
        shieldCollider.gameObject.tag = "Corpse";
        this.tag = "Corpse";
        shieldCollider.gameObject.layer = LayerMask.NameToLayer("Corpse");
        Destroy(chargingBullet);
    }
}
