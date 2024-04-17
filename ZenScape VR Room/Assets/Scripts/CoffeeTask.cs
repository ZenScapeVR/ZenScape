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
    public TextMeshPro metrics;
    private int coffeesDrank;
    private int coffeesSpilt;
    private int attempts;
    public int status;
    public int lastStatus = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (status != lastStatus)
        {
            if (status == 1)
                DrankCoffee();
            else if (status == 2)
                SpiltCoffee();
            lastStatus = status;
        }
    }

    void UpdateMetrics()
    {
        //if (attempts != 0)
        //{
        //    //accuracy = (float)correct / (float)attempts;
        //}
        //else
        //{
        //    //accuracy = 1f;
        //}

        // Update UI text
        metrics.text = "Coffees Drank: " + coffeesDrank + "\nCoffees Spilt: " + coffeesSpilt;
    }

    public void DrankCoffee()
    {
        coffeesDrank++;
        UpdateMetrics();
    }

    public void SpiltCoffee()
    {
        coffeesSpilt++;
        UpdateMetrics();
    }

    public void ResetMetrics()
    {
        coffeesSpilt = 0;
        coffeesDrank = 0;
        UpdateMetrics();
    }
    

    public void StartTask()
    {
        Instantiate(CoffeeMugObject, SpawnMug.transform.position, Quaternion.identity);
        attempts++;
        //CoffeeMugObject.GetComponent<Material>() = Grey;

    }

    

    public CoffeeMugProperties CoffeeMug()
    {
        var mug = new CoffeeMugProperties();
        numberOfMugs++;
        // need to spawn mugs in
        return mug;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hands")
            StartTask();
    }
}
