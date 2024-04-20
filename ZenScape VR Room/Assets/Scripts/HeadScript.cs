using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class HeadScript : MonoBehaviour
{

    public int status;
    private int lastStatus;
    public TextMeshPro metrics;
    public AudioSource audioSource;
    public AudioClip success;
    public AudioClip failure;
    public int coffeesDrank = 0;
    public int coffeesSpilt = 0;

    // Start is called before the first frame update
    void Start()
    {
        status = 0;
        lastStatus = 0;
    }

    // Update is called once per frame
    void Update()
    {

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CoffeeMug")
        {
            Material material = other.GetComponent<Renderer>().sharedMaterial;
            string mugColor = material.name;
            Debug.Log($"mug material {material} is the color {mugColor}");
            switch (mugColor)
            {
                case "RedDark":
                    Debug.Log("Coffee is hot.");
                    PlaySound(failure);
                    SpiltCoffee();
                    Destroy(other.gameObject);
                    break;
                case "GreenDark":
                    Debug.Log("Coffee is perfect.");
                    PlaySound(success);
                    DrankCoffee();
                    Destroy(other.gameObject);
                    break;
                case "Blue":
                    Debug.Log("Coffee is cold.");
                    PlaySound(failure);
                    SpiltCoffee();
                    Destroy(other.gameObject);
                    break;
                case "Grey":
                    Debug.Log("Mug is empty.");
                    break;
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
