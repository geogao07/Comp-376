using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_enable : MonoBehaviour
{

    public GameObject Drone;
    public GameObject player_1;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            Drone.SetActive(true);
      
        }



    }
}
