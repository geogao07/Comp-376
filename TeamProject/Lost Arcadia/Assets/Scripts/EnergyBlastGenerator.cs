using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBlastGenerator : MonoBehaviour
{
    public GameObject prefab;
    private EnergyBarController energyBar;

    private float energyCost;
    private float timer;

    private IrisController iris;

    void Start()
    {
        energyBar = GetComponent<EnergyBarController>();
        energyCost = prefab.GetComponent<EnergyBlast>().energyCost;
        timer = 0;
        iris = GameObject.Find("Player/Iris").GetComponent<IrisController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (energyBar.canSpendEnergy(energyCost))
            {
                iris.UseSkill();
                energyBar.spendEnergy(energyCost);
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
    }
}
