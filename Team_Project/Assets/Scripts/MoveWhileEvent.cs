using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MoveWhileEvent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool blockMovement = false;
    public Flowchart flowchart;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (blockMovement)
        {
            bool playing = false;
            try {
                playing = flowchart.GetBooleanVariable("playing");
            }
            catch { playing = false; }

            if (playing)
            {
                IrisController.canMove = false;
            }
            else IrisController.canMove = true;
        }
    }
}
