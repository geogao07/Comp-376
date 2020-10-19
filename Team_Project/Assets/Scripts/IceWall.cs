using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWall : MonoBehaviour
{
    public float timeout;
    public float energyCost;
    public bool isPreview = false;

    void Start()
    {
        timeout = timeout == 0 ? 3 : timeout;
    }

    void Update()
    {
        if (isPreview == false)
        {
            timeout -= Time.deltaTime;
            if (timeout <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // if (isPreview == false)
       // {
            if (collision.collider.tag == "Player" || collision.collider.tag == "Enemy")
            {
           
                Destroy(gameObject);
            }
        //}
    }
  
}
