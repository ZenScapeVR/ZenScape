using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClockDistraction : MonoBehaviour
{
    public GameObject alarm_clock;
    public AudioSource clock_source;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEvent()
    {
        // play audio
        alarm_clock.GetComponent<AudioSource>().Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hands")
        {
            Debug.Log("touched alarm clock");
            alarm_clock.GetComponent<AudioSource>().Stop();
        }
    }
}
