using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWallGenerator : MonoBehaviour
{
    public GameObject iceWallPrefab;

    private GameObject preview;
    private Vector3 firstPoint;
    private float energyCost;
    private EnergyBarController energyBar;
    private IrisController iris;

    void Start()
    {
        energyBar = GetComponent<EnergyBarController>();
        energyCost = iceWallPrefab.GetComponent<IceWall>().energyCost;
        iris = GameObject.Find("Player/Iris").GetComponent<IrisController>();
    }

    void Update()
    {
        MouseDetection();
    }

    void MouseDetection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            preview = Instantiate(iceWallPrefab, pos, Quaternion.identity);
            preview.GetComponent<IceWall>().isPreview = true;
            Color currentColor = preview.GetComponent<SpriteRenderer>().color;
            preview.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a / 2);
            preview.GetComponent<Collider2D>().enabled = false;
            firstPoint = pos;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (energyBar.canSpendEnergy(energyCost))
            {
                energyBar.spendEnergy(energyCost);
                GameObject iceWall = Instantiate(iceWallPrefab, preview.transform.position, preview.transform.rotation);
                iceWall.transform.localScale = preview.transform.localScale;
                iceWall.GetComponent<IceWall>().isPreview = false;
                iris.UseSkill();
            }
            Destroy(preview);
            preview = null;
        }

        if (Input.GetAxis("Mouse X") != 0 && Input.GetAxis("Mouse Y") != 0)
        {
            if (preview != null)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 currentVec = pos - firstPoint;
                float angle = Vector3.SignedAngle(Vector3.right, currentVec, Vector3.forward) + 180 + 22.5f;

                int rotate = (int)angle / 45;
                preview.transform.rotation = Quaternion.identity;
                preview.transform.RotateAround(firstPoint, Vector3.forward, rotate * 45);
            }
        }
    }
}
