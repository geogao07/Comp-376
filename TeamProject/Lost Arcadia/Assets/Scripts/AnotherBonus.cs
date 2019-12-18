using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnotherBonus : MonoBehaviour
{
    // Start is called before the first frame update
    public float canPickRadius = 1f;
    private bool canPick;
    private GameObject player;

    void Start()
    {
        Text pick = transform.GetComponentInChildren<Text>();
        pick.text = "";
        canPick = false;
        player = GameObject.Find("Player/Iris");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = player.transform.position.x;
        float vertical = player.transform.position.y;
        if (Mathf.Abs(vertical - transform.position.y) <= canPickRadius && Mathf.Abs(horizontal - transform.position.x) <= canPickRadius)
        {
            transform.GetComponentInChildren<Text>().text = "E";
            canPick = true;
        }

        else
        {
            transform.GetComponentInChildren<Text>().text = "";
            canPick = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && canPick == true)
        {
            Destroy(gameObject);
            DroneController.D.speedUp = false;
            DroneController.D.knockBack = false;
            DroneController.D.pierce = false;
            DroneController.D.powerUp = false;
            

            DroneController.D.specialBulletsNum = 24;

            DroneController.D.speedUp = true;
            //DroneController.D.knockBack = true;
            //DroneController.D.pierce = true;
            //DroneController.D.powerUp = true;
        }

    }
}
