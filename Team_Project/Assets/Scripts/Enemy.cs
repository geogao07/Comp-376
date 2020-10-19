using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int contactDamage;
    public Animator anim;
    public bool canKnockBack = true;
    private Vector2 originalSpeed;
    private float knockBackTime = 0.05f;
    
    void Start()
    {
        originalSpeed = this.GetComponent<Rigidbody2D>().velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(int damage)
    {
        if (hp > 0)
        {
            if (GetComponent<SpriteRenderer>() != null) gameObject.AddComponent<DamageFlash>();
            else GetComponentInChildren<SpriteRenderer>().gameObject.AddComponent<DamageFlash>();
        }

        hp -= damage;

        if (hp <= 0)
        {
            Death();
        }
    }
    public virtual void Death()
    {
        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<IrisController>().Damage(contactDamage, col.transform.position - transform.position);
        }
    }

    public void callResetSpeed()
    {
        Invoke("resetSpeed", knockBackTime);
    }

    private void resetSpeed()
    {
        this.GetComponent<Rigidbody2D>().velocity = originalSpeed;
        canKnockBack = true;
    }


    public virtual void ApplyEnergyBlast(Vector2 direction)
    {
        
    }

}
