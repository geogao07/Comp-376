using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField]
    float camWeight; // Percentage position at which the camera tries to center between Iris and Crosshair.
    [SerializeField]
    float camSmooth; // How quickly it tries to follow the ideal position.
    [SerializeField]
    float verticalOffset; // How much over the center position it tries to go. To avoid looking too much into the ground.

    [SerializeField]
    Sprite crosshairSprite1;
    [SerializeField]
    Sprite crosshairSprite2;
    [SerializeField]
    Sprite crosshairSprite3;

    GameObject iris;
    IceWallGenerator iceWallGenerator;
    EnergyBlastGenerator energyBlastGenerator;
    SpriteRenderer renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        iris = transform.parent.Find("Iris").gameObject;
        iceWallGenerator = GetComponent<IceWallGenerator>();
        energyBlastGenerator = GetComponent<EnergyBlastGenerator>();
        iceWallGenerator.enabled = false;
        energyBlastGenerator.enabled = false;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 crosshairPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshairPos.z = transform.position.z;
        transform.position = crosshairPos;
        Vector3 camPos = Vector3.Lerp(iris.transform.position, transform.position, camWeight);
        camPos.z = Camera.main.transform.position.z;
        camPos.y += verticalOffset;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, camPos, camSmooth * Time.deltaTime);
        keyBoardDetection();
    }

    void keyBoardDetection()
    {
        if (Input.GetKeyDown("1"))
        {
            iceWallGenerator.enabled = false;
            energyBlastGenerator.enabled = true;
            renderer.sprite = crosshairSprite1;
        }

        if (Input.GetKeyDown("2"))
        {
            energyBlastGenerator.enabled = false;
            iceWallGenerator.enabled = true;
            renderer.sprite = crosshairSprite2;
        }

        if (Input.GetKeyDown("3"))
        {
            renderer.sprite = crosshairSprite3;
        }
    }
}
