using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenscapeTimer : MonoBehaviour
{
    public float TimeRemaining = 0;

    public ZenscapeTimer(float timeRemaining)
    {
        this.TimeRemaining = timeRemaining;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
        } else
        {
            TimeRemaining = 0;
        }
    }

    public float DisplayTime(float timerTime)
    {
        float minutes = Mathf.FloorToInt(timerTime / 60);
        return minutes;
    }
}
