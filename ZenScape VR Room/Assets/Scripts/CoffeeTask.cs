using System.Diagnostics;
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
        if(numberOfMugs == 0){
            Instantiate(CoffeeMugObject, SpawnMug.transform.position, Quaternion.identity);
        }else{
            UnityEngine.Debug.Log("Too many coffee mugs, only one at a time is allowed.");
        }
        //CoffeeMugObject.GetComponent<Material>() = Grey;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hands")
            StartTask();
    }
}
