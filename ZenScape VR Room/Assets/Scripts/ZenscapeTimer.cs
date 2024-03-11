using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenscapeTimer : MonoBehaviour
{
    public float TimeRemaining = 0;
    public bool TimerOn = false;

    public ZenscapeTimer(float timeRemaining)
    {
        this.TimeRemaining = timeRemaining;
        this.TimerOn = true;
    }

    private void Start()
    {
        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn && TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
        } else
        {
            TimeRemaining = 0;
            TimerOn = false;
        }
    }

    public float DisplayTime(float timerTime)
    {
        float minutes = Mathf.FloorToInt(timerTime / 60);
        return minutes;
    }
}
