using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Button_Control : MonoBehaviour
{
    public GameObject Button_E;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.C))
        {
            Button_E.SetActive(false);
        }

    }
}
