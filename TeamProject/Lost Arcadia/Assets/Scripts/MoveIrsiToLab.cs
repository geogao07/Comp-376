using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MoveIrsiToLab : MonoBehaviour
{
    [SerializeField] Transform move;
    GameObject iris;
    private void Start()
    {
        iris = GameObject.Find("Player/Iris");
    }
    private void Update()
    {
        if(iris.transform.position.x > move.position.x) Invoke("MoveIris", 1f);

    }
    // Start is called before the first frame update

    private void MoveIris()
    {
        //move to the lab
        Debug.Log("move iris");
        SceneManager.LoadScene("stage 2-3");
    }
}
