using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_eleva_cont : MonoBehaviour
{
    public GameObject next_room;
    public GameObject key_c;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            next_room.SetActive(true);
            key_c.SetActive(true);
        }

 

    }
}
