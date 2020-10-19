using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakedDrone : MonoBehaviour
{

    public GameObject fakedrone;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            Destroy(fakedrone);
        }
    }
}
