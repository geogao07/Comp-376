using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Iris_lab_position : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject iris_lab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (iris_lab.transform.localPosition.x > 19.0)
        {
            SceneManager.LoadScene(7);
        }

        Debug.Log(iris_lab.transform.localPosition.x);


    }
}
