using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoffeeTask : MonoBehaviour
{
    int numberOfMugs = 0;
    GameObject CoffeePot;
    public GameObject CoffeeMugObject;
    public bool TaskStarted;
    public GameObject SpawnMug;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void StartTask()
    {
        Instantiate(CoffeeMugObject, SpawnMug.transform.position, Quaternion.identity);
        //CoffeeMugObject.GetComponent<Material>() = Grey;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hands")
            StartTask();
    }
}
