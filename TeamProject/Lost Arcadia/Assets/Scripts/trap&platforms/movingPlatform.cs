using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public bool moveVertical = false;
    public bool moveHorizontal=false;
    public bool special = false;
    public int speed;
    public float top;
    public float bottom;
    public float right;
    public float left;
    public bool onPlatform = false;

    private int speedBackup;
    void Start()
    {
        speedBackup = speed;
    }

    // Update is called once per frame
    void Update()
    {
        move();
        changeDirection();
    }

    void move()
    {
        if (moveVertical)
        {
            Vector3 pos = transform.position;
            pos.y += Time.deltaTime * speed;
            transform.position = pos;
        }
        else if (moveHorizontal)
        {
            Vector3 pos = transform.position;
            pos.x += Time.deltaTime * speed;
            transform.position = pos;
        }
        else if (special&&onPlatform)
        {
            if (IrisController.iris.transform.position.x > transform.position.x&& transform.position.x<right)
            {
                Vector3 pos = transform.position;
                pos.x += Time.deltaTime * speed;
                transform.position = pos;
            }
            else if (IrisController.iris.transform.position.x < transform.position.x && transform.position.x > left)
            {
                Vector3 pos = transform.position;
                pos.x -= Time.deltaTime * speed;
                transform.position = pos;
            }
        }
    }

    void changeDirection()
    {
        if (moveVertical)
        {
            if (transform.position.y > top)
                speed =-speedBackup;
            else if(transform.position.y < bottom)
                speed = speedBackup;
        }
        else if (moveHorizontal)
        {
            if (transform.position.x > right)
                speed = -speedBackup;
            else if (transform.position.x < left)
                speed = speedBackup;
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.transform.SetParent(transform);
            onPlatform = true;
            IrisController.iris.onPlat = true;
        }
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.transform.SetParent(null);
            onPlatform = false;
            IrisController.iris.onPlat = false;
        }
    }
}
