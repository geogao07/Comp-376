using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class labTransition : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] GameObject preLab;
    private GameObject iris;
    private float startX;
    private float endX;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        iris = GameObject.Find("Player/Iris");
        startX = start.position.x;
        endX = end.position.x;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IrisController.hasCard)
        {
            startDimming();
            preLab.SetActive(false);
        }
        else
        {
            preLab.SetActive(true);
        }

    }
    private void startDimming()
    {
        if (iris.transform.position.x> startX)
            {
            Color dim = new Color(0f, 0f, 0f, 0f);
            float alphaValue = (iris.transform.position.x - startX) / (endX - startX);
            dim.a = alphaValue;
            sr.color = dim;
            sr.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Fungus.Flowchart.BroadcastFungusMessage("cardEvent");
        }
    }
}
