using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class bossSceneManager : MonoBehaviour
{
    public HealthBarController hbController;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            Fungus.Flowchart.BroadcastFungusMessage("start");

        }
        catch { }
        hbController = GameObject.Find("Player/Iris").GetComponent<HealthBarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hbController.isDead())
        SceneManager.LoadScene("stage 2-3");
    }
}
