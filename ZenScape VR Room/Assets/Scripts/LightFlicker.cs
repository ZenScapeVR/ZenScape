using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public bool isFlickering = false;
    public float timeDelay;
    public ZenscapeTimer timer;
    public int counter = 0;
    public int finalCount;

    private void Start()
    {
        finalCount = Random.Range(30, 45);
        StartFlicker();
    }

    void Update()
    {
        if (isFlickering)
            StartCoroutine(FlickeringLight());

        if (counter >= finalCount)
            StopFlicker();
    }

    IEnumerator FlickeringLight()
    {
        counter++;
        this.gameObject.GetComponent<Light>().enabled = true;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        this.gameObject.GetComponent<Light>().enabled = false;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
    }

    public void StartFlicker()
    {
        counter = 0;
        isFlickering = true;
    }

    public void StopFlicker()
    {
        isFlickering = false;
        this.gameObject.GetComponent<Light>().enabled = false;
    }
}
