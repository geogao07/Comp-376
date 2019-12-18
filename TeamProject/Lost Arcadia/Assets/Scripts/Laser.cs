using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 3f;
    Rigidbody2D rigi;

    Vector3 position;
    Vector3 normal;
    public int damage = 2;

    public bool pierce = false;
    public bool knockBack = false;
    public int knockBackVelocity = 3;
    //public GameObject laserPrefab;

    Vector3 direction;

    void Start()
    {
        rigi = transform.GetComponent<Rigidbody2D>();
        position = transform.position;
    }

    void Update()
    {
        rigi.velocity = (direction.normalized * speed);
    }

    public void setDirection(Vector3 di)
    {
        direction = di.normalized;
    }

    private void FixedUpdate()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 30f, 1 << LayerMask.NameToLayer("IceWall"));

        if (hit.collider != null)
        {
            position = hit.point;
            normal = hit.normal;
            //Debug.Log(hit.collider.name);
        }
    }




    void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (coll.tag == "IceWall")
        {
            Vector3 newDir = Vector3.Reflect(direction, normal);
            //Quaternion rotation = Quaternion.FromToRotation(Vector2.right, newDir);
            //transform.rotation = rotation;
            direction = newDir;
        }
        if ((tag == "Laser-Ally" && coll.tag == "Shield") || coll.tag == "Ground")
        {
            KillEmission();
            return;
        }
        else if (coll.tag == "Enemy"&& tag == "Laser-Ally")
        {
            if (!pierce)
            {
                KillEmission();
            }
            Enemy enemy = coll.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            if (knockBack == true && enemy.canKnockBack == true)
            {
                enemy.canKnockBack = false;
                Vector2 moveTowards = direction;
                moveTowards.y = 0;
                moveTowards.Normalize();
                enemy.GetComponent<Rigidbody2D>().velocity = moveTowards * knockBackVelocity;
                enemy.callResetSpeed();
            }
        }
        else if (tag == "Laser-Enemy" && coll.tag == "Player")
        {
            coll.GetComponent<IrisController>().Damage(damage, coll.transform.position - transform.position);
        }
    }

    private void KillEmission()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(transform.Find("ball").gameObject);
        var em = transform.Find("afterimage").GetComponent<ParticleSystem>().emission;
        em.rateOverTime = 0.0f;
        var em2 = transform.Find("light").GetComponent<ParticleSystem>().emission;
        em2.rateOverTime = 0.0f;
    }
}

