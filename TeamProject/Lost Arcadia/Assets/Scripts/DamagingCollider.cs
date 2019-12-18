using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingCollider : MonoBehaviour
{
    private int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
       if (col.tag == "Player")
        {
            col.GetComponent<IrisController>().Damage(damage, col.transform.position - transform.position);

            //player takeDamage
        }
    }
    
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<IrisController>().Damage(damage, col.transform.position - transform.position);

            //player takeDamage
        }
    }
}
