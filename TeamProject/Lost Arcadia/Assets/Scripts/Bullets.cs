using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10f;
    public int damage = 1;
    public bool pierce=false;
    public bool knockBack=false;
    public int knockBackVelocity = 3;
    
    private float sizeScale=1; //control size
    //private Color c;

    private Vector3 direction;
    Rigidbody2D rigi;

    void Start()
    {
        rigi = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigi.velocity = direction * speed;
    }

    public void setDirection(Vector3 di)
    {
        direction = di.normalized;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (coll.tag == "Enemy")
        {
            //Debug.Log("hit enemy");
            if (!pierce)
            {
                Destroy(gameObject);
            }
            Enemy enemy = coll.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            if (knockBack == true&&enemy.canKnockBack==true)
            {
                enemy.canKnockBack = false;
                Vector2 moveTowards = direction;
                moveTowards.y = 0;
                moveTowards.Normalize();
                enemy.GetComponent<Rigidbody2D>().velocity = moveTowards * knockBackVelocity;
                enemy.callResetSpeed();
            }
        }
        if (coll.tag == "IceWall" || coll.tag == "Ground" || coll.tag == "Shield")
        {
            Destroy(gameObject);
        }
        if (coll.tag == "Player")
        {
            coll.GetComponent<IrisController>().Damage(damage, coll.transform.position - transform.position);
            Destroy(gameObject);
        }
        if (coll.tag == "EnergyBlast"&&this.tag!= "Bullet-Ally")
        {
            Vector3 newDir = new Vector3(-direction.x, Random.value, direction.z);
            newDir.Normalize();
            
            direction = newDir;
        }

    }


}
