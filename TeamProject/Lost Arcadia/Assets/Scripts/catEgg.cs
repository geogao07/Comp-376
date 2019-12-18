using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catEgg : MonoBehaviour
{
    [SerializeField] int damage = 3;
    [SerializeField] GameObject catPrefab;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(Random.Range(4f,7f), -5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 newPosition = transform.position;
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IrisController>().Damage(damage, collision.transform.position - newPosition); Destroy(gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Ground")
        {
            newPosition.y += 1f;
            GameObject cat = Instantiate(catPrefab, newPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
