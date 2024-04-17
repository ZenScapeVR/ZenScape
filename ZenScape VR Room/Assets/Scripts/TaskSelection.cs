using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int Day = 1;
    public bool AllTasksComplete = false;
    public GameObject Task;
    public ZenscapeTimer timer;

    void Start()
    {
        Day = 1;
        AllTasksComplete = false;
        timer.TimeRemaining = 240; // 4 minutes
    }

    void Update()
    {
        // for each task (game object), we need to check if complete to move to next day (level)
        if(AllTasksComplete)
        {
            Day++;
            timer.TimeRemaining = 240;
            // end current level
        }
        SelectedTask();
    }

    void SelectedTask()
    {
        switch (Day)
        {
            case 1:
                // first day/level, easy day, spawn tasks so user gets a feel for the game
                break;
            case 2:
                // second day, bump up task frequency
                break;
            case 3:
                // third day, bump up task frequency
                break;
            case 4:
                // end of game, go to final stats screen
                break;
        }
    }
}
