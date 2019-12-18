using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_enable : MonoBehaviour
{
    public GameObject letter_g;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Z))
        {
            letter_g.SetActive(true);
        }

        else
        {
            letter_g.SetActive(false);
        }
    }
}
