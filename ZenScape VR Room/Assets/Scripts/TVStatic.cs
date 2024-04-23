using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVStatic : MonoBehaviour
{
    public GameObject screen;
    public ZenscapeTimer timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timer.TimeRemaining <= 0)
            StopTask();
    }

    public void StartTask() 
    {
        screen.GetComponent<VideoPlayer>().Play();
        timer.TimeRemaining = 15;
        
    }

    public void StopTask()
    {
        screen.GetComponent <VideoPlayer>().Stop();
    }
}
