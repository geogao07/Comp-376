using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderGroundControl : MonoBehaviour
{
    public GameObject leave;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            leave.SetActive(true);
        }
    }
}
