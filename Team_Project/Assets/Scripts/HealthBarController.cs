using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public float MaxHealth;
    private Slider slider;

    void Start()
    {
        slider = GameObject.Find("UI/HealthBar").GetComponent<Slider>();
    }

    public void updateHP(float demage)
    {
        slider.value = (slider.value * MaxHealth - demage) / MaxHealth;
    }

    public bool isDead()
    {
        //Debug.Log("health" + slider.value);
        return slider.value == 0;
    }

    public void reset()
    {
        slider.value = 1;
    }
}
