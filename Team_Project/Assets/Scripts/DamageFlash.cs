using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private float duration;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().material.color = new Color(2f, 2f, 2f, 1f);
        duration = 0.07f;
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<SpriteRenderer>().material.color = new Color(5f, 5f, 5f, 1f);
        duration -= Time.deltaTime;
        if (duration <= 0.0f)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
            Destroy(this);
        }
    }
}
