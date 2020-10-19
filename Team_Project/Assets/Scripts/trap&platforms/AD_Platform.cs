using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Platform : MonoBehaviour
{
    // Start is called before the first frame update
    public float appearTime;
    public float disappearTime;

    private GameObject platform;
    private float timer = 0;
    void Start()
    {
        platform=transform.Find("plat").gameObject;
        platform.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > appearTime)
            platform.SetActive(false);
        if (timer > appearTime + disappearTime)
        {
            platform.SetActive(true);
            timer = 0;
        }
    }
}
