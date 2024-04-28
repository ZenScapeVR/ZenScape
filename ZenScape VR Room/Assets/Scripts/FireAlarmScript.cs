using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FireAlarmScript : MonoBehaviour
{
    [SerializeField] private AudioSource alarm;
    [SerializeField] private Light light;
    [SerializeField] private ZenscapeTimer timer;
    [SerializeField] private int intensity = 0;
    [SerializeField] private float duration = 0;
    [SerializeField] private Animator animation;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        animation.SetBool("Flashing Alarm Light", false);
        //StartEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.TimeRemaining <= 0 && isActive)
            EndEvent();
    }

    public void StartEvent()
    {
        isActive = true;
        alarm.Play();
        // activate light flashing on and off
        light.enabled = true;
        animation.SetBool("Flashing Alarm Light", true);
        timer.TimeRemaining = 15;
    }

    public void EndEvent()
    {
        alarm.Stop();
        // deactivate light
        animation.SetBool("Flashing Alarm Light", false);
        light.enabled = false;
        isActive = false;
    }
}
