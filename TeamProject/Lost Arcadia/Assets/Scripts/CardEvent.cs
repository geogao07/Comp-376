using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEvent : MonoBehaviour
{

	// Start is called before the first frame update
	[SerializeField] GameObject card;
	public Sprite sprite;
    // Start is called before the first frame update

    public AudioSource pickup;

	void Start()
	{
        

	}

	// Update is called once per frame
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag == "Player")
        {
            pickup.Play();
            card.SetActive(true);
            card.GetComponent<SpriteRenderer>().sprite = sprite;
            Fungus.Flowchart.BroadcastFungusMessage("cardEvent");
            IrisController.hasCard = true;
            Destroy(gameObject);
        }
	}
}
