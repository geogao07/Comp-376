using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarController : MonoBehaviour
{
    [SerializeField]
    private float rechargeDelay;
    [SerializeField]
    private float rechargeSpeed;

    public float MaxEnergy;
    private Slider slider;

    private float timeRemaining;
    private float currentEnergy;

    void Start()
    {
        slider = GameObject.Find("UI/EnergyBar").GetComponent<Slider>();
        timeRemaining = 0.0f;
        currentEnergy = MaxEnergy;
    }

    void Update()
    {
        if (timeRemaining <= 0.0f && currentEnergy < MaxEnergy)
        {
            currentEnergy = Mathf.Min(currentEnergy + rechargeSpeed * Time.deltaTime, MaxEnergy);
            updateBar();
        }
        else if (timeRemaining > 0.0f)
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    private void updateBar()
    {
        slider.value = currentEnergy / MaxEnergy;
    }

    public bool spendEnergy(float cost)
    {
        if (currentEnergy < cost) return false;
        currentEnergy -= cost;
        updateBar();
        timeRemaining = rechargeDelay;
        return true;
    }

    public bool canSpendEnergy(float cost)
    {
        return (currentEnergy >= cost);
    }
}
