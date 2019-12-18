using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBlast : MonoBehaviour
{
    public AudioClip bulletsound;
    public AudioSource bulletsoundsourse;
    public AudioClip lasersound;
    public AudioSource lasrsoundsourse;
    public AudioClip icewallsound;
    public AudioSource icewallsoundsourse;
    public float timeout;
    public float energyCost;

    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        timeout = timeout == 0 ? 5 : timeout;
    }

    // Update is called once per frame
    void Update()
    {
        timeout -= Time.deltaTime;

        if (timeout <= 0)
        {
            Destroy(gameObject);
        }

        if (Input.GetMouseButtonUp(1))
        {
            
            icewallsoundsourse.clip = icewallsound;
            icewallsoundsourse.Play();
        }

        if (Input.GetMouseButtonDown(0))
        {
            startTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0) && (Time.time-startTime)>0.5f)
        {
            lasrsoundsourse.clip = lasersound;
            Debug.Log("laser");
            lasrsoundsourse.Play();
        }

        if (Input.GetMouseButtonUp(0) && (Time.time - startTime) < 0.5f)
        {
            bulletsoundsourse.clip = bulletsound;
            Debug.Log("bullet");
            bulletsoundsourse.Play();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Vector2 direction = collision.collider.transform.position - transform.position;
            collision.collider.GetComponent<IrisController>().ApplyEnergyBlast(direction);
        } else if (collision.collider.tag == "Enemy")
        {
            Vector2 direction = collision.collider.transform.position - transform.position;
            collision.collider.GetComponent<Enemy>().ApplyEnergyBlast(direction);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Vector2 direction = collision.gameObject.transform.position - transform.position;
            collision.gameObject.GetComponent<Enemy>().ApplyEnergyBlast(direction);
        }

    }
}
