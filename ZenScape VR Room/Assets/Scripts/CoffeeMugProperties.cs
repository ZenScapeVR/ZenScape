using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CoffeeMugProperties : MonoBehaviour
{
    public Material Red;
    public Material Green;
    public Material Blue;
    public Material Grey;
    public ZenscapeTimer timer;
    public int status = 0;



    void Start()
    {

    }

    private void Update()
    {
        if(timer.TimeRemaining <= 30 && timer.TimeRemaining >= 28)
            gameObject.GetComponent<Renderer>().sharedMaterial = Green;
        if (timer.TimeRemaining <= 15 && timer.TimeRemaining >= 13)
            gameObject.GetComponent<Renderer>().sharedMaterial = Blue;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "CoffeePot")
            PourCoffee();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Head")
        {
            Material material = gameObject.GetComponent<Renderer>().sharedMaterial;
            string mugColor = material.name;
            Debug.Log($"mug material {material} is the color {mugColor}");
            switch (mugColor)
            {
                case "RedDark":
                    Debug.Log("Coffee is hot.");
                    status = 2;
                    Destroy(gameObject);
                    break;
                case "GreenDark":
                    Debug.Log("Coffee is perfect.");
                    status = 1;
                    Destroy(gameObject);
                    break;
                case "Blue":
                    Debug.Log("Coffee is cold.");
                    status = 2;
                    Destroy(gameObject);
                    break;
                case "Grey":
                    Debug.Log("Mug is empty.");
                    status = 0;
                    break;
            }
        }
    }

    public void PourCoffee()
    {
        // change color and boolean
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (gameObject.GetComponent<Renderer>() == null)
        {
            renderer = gameObject.AddComponent<Renderer>();
        }
        renderer.sharedMaterial = Red;
        Debug.Log($"renderer material = {renderer.sharedMaterial}");
        timer.TimeRemaining = 60;
    }
}
