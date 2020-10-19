using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoomController : MonoBehaviour
{
    private Enemy beetleMonster;
    private Enemy[] bottomBeetles;
    private GameObject ground;

    public GameObject key_c;

    // Start is called before the first frame update
    void Start()
    {
        beetleMonster = GameObject.Find("BossRoom/BeetleMonster").GetComponent<Enemy>();
        ground = GameObject.Find("BossRoom/Base");
        Enemy topLeft = GameObject.Find("BossRoom/BeetleMonster_topleft").GetComponent<Enemy>();
        Enemy topRight = GameObject.Find("BossRoom/BeetleMonsterTopRight").GetComponent<Enemy>();
        Enemy bottomLeft = GameObject.Find("BossRoom/BeetleMonsterLeftDown").GetComponent<Enemy>();
        Enemy bottomRight = GameObject.Find("BossRoom/BeetleMonsterRightDown").GetComponent<Enemy>();
        bottomBeetles = new Enemy[4] { topLeft, topRight, bottomLeft, bottomRight };
    }

    // Update is called once per frame
    void Update()
    {
        if (beetleMonster.hp <= 0 && ground != null)
        {
            Destroy(ground);
            Destroy(beetleMonster);
        }

        if (IsBottomBeetlesDead())
        {
           key_c.SetActive(true);
        }
    }

    bool IsBottomBeetlesDead()
    {
        foreach(Enemy beetle in bottomBeetles)
        {
            if (beetle.hp > 0)
            {
                return false;
            }
        }
        return true;
    }
}
