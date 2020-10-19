using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMonster : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody;
    public bool facingRight = false;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody.velocity = new Vector2(moveSpeed, 0f);
       
         CheckFlip();

    }

    private void CheckFlip()
    {
        if (player.transform.position.x < transform.position.x && facingRight)

        {
            Flip();
        }
        if (player.transform.position.x > transform.position.x && !facingRight)

        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 tmpScale = transform.localScale;
        tmpScale.x *= -1;
        transform.localScale = tmpScale;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
    }
}
      
    
