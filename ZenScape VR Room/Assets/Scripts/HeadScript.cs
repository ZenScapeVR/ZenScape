using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class HeadScript : MonoBehaviour
{
    public int status;
    private int lastStatus;
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
        // Find the Coffee game object and call its methods there.
        CoffeeGameParent coffeeGame = GameObject.FindObjectOfType<CoffeeGameParent>();
        if (coffeeGame != null)
        {
            UnityEngine.Debug.Log("coffeeGame PARENT OBJ: " + coffeeGame.name);
            coffeeGame.MetricsAndEndGame(coffeesDrank);
        }else{
            UnityEngine.Debug.LogError("CoffeeGame object not found!");
        }
    }

    public void DrankCoffee(bool correct)
    {
        if(correct){
            coffeesDrank++;
        }
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
                    DrankCoffee(false);
                    Destroy(other.gameObject);
                    break;
                case "GreenDark":
                    Debug.Log("Coffee is perfect.");
                    PlaySound(success);
                    DrankCoffee(true);
                    Destroy(other.gameObject);
                    break;
                case "Blue":
                    Debug.Log("Coffee is cold.");
                    PlaySound(failure);
                    DrankCoffee(false);
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
