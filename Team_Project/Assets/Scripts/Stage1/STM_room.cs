using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STM_room : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject transformed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            transformed.SetActive(true);
        }
    }
}
