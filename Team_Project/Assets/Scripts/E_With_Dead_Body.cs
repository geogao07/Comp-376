using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_With_Dead_Body : MonoBehaviour
{
    // Start is called before the first frame update
    public int distance = 1;
    public int monsterType;

    private GameObject e;
    private GameObject shield;
    private GameObject player;
    void Start()
    {
        e = transform.Find("e").gameObject;
        e.SetActive(false);
        player = GameObject.Find("Player/Iris");
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.GetComponent<SpiderMonsterScript>() == null)
        //{
            if (this.transform.localScale.x == 1 && this.tag != "Corpse")
            {
                e.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (this.transform.localScale.x == -1 && this.tag != "Corpse")
                e.transform.localEulerAngles = new Vector3(0, 0, 180);
        //}
        //else
        //{
        //    if (this.GetComponent<SpiderMonsterScript>().facingRight)
        //    {
        //        e.transform.localEulerAngles = new Vector3(0, 180, 0);
        //    }
        //}
        

        float horizontal = IrisController.iris.transform.position.x;
        float vertical = IrisController.iris.transform.position.y;

        if (Mathf.Abs(vertical - transform.position.y) <= distance && Mathf.Abs(horizontal - transform.position.x) <= distance&& this.gameObject.tag== "Corpse")
        {
           e.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(gameObject);
                DroneController.D.speedUp = false;
                DroneController.D.knockBack = false;
                DroneController.D.pierce = false;
                DroneController.D.powerUp = false;

                DroneController.D.specialBulletsNum = 24;

                switch (monsterType)
                {
                    case 1:
                        DroneController.D.speedUp = true;
                        break;
                    case 2:
                        DroneController.D.knockBack = true;
                        break;
                    case 3:
                        DroneController.D.pierce = true;
                        break;
                    case 4:
                        DroneController.D.powerUp = true;
                        break;
                }

            }
        }
        else
        {
            transform.Find("e").gameObject.SetActive(false);
        }
        
    }
}
