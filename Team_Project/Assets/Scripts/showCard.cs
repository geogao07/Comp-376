﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showCard : MonoBehaviour
{
    [SerializeField] GameObject card;

   
    // Start is called before the first frame update
    void Start()
    {
        
        card.SetActive(false);

    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") card.SetActive(true);
        

    }
}
