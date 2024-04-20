using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTest : MonoBehaviour
{
    public GameObject lamp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hands")
        {
            Debug.Log("Hands touched the butt");
            lamp.GetComponent<LightFlicker>().StartFlicker();
        }
           
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hands")
        {
            lamp.GetComponent<LightFlicker>().StartFlicker();
        }

    }
}
