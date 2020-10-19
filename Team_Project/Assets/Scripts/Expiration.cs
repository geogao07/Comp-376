using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expiration : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float maxTime = 3f;
    private float mTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mTime += Time.deltaTime;
        if (mTime > maxTime)
            Destroy(gameObject);
    }
}
