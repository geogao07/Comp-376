using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
public class stage2Events : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string triggerName = collision.gameObject.transform.name;

        if (triggerName.Contains("Stage2-1event"))
        {
            Fungus.Flowchart.BroadcastFungusMessage(triggerName);
            collision.enabled = false;
        }
    }
}
