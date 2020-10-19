using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    public void playgame()
    {


        SceneManager.LoadScene(1);


    }

    public void playgame2()
    {


        SceneManager.LoadScene(2);


    }

    

    public void exit()
    {


        SceneManager.LoadScene(3);


    }
}
