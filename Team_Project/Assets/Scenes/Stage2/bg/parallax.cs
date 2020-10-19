using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public GameObject cam;
    public float parallaxEffect;
    private float length, startPosX,startPosY;

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;

        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = cam.transform.position.x * parallaxEffect;
        float distY = Mathf.Clamp(cam.transform.position.y * parallaxEffect,0f,25f);
        transform.position = new Vector3(startPosX + dist, startPosY+distY, transform.position.z);

        float temp = cam.transform.position.x * (1 - parallaxEffect);
        if (temp > startPosX + length)
        {
            startPosX += length;
        } else if (temp < startPosX - length){
            startPosX -= length; 
        }
    }
}
