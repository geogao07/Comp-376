using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_control : MonoBehaviour
{
    public GameObject button_C;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.C))
        {
            button_C.SetActive(true);
        }




    }
}
