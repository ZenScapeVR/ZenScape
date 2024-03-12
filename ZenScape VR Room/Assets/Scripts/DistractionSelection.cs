using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionSelection : MonoBehaviour
{
    public int Difficulty = 0;
    public float HearRate = 0f;
    public string SelectedDistration = null;
    private enum DIFFICULTY_LEVEL
    {
        EASY,
        MEDIUM,
        HARD
    };
    public GameObject Distraction;
    // Can set all game objects here to call when selected

    void Start()
    {
        // remove once implemented
        GetHeartRate();
    }

    void Update()
    {
        // check ZenscapeTimer and call GetHeartRate() at certain times
    }
    
    // select distraction
    public void GetHeartRate()
    {
        DIFFICULTY_LEVEL level = DIFFICULTY_LEVEL.EASY;
        
        // may need to change values after testing, these are temporary
        // high heartrate
        if (HearRate >= 88 || Difficulty == 3) { level = DIFFICULTY_LEVEL.HARD; }
        // medium heartrate
        else if (HearRate >=  74 || Difficulty == 2)  { level = DIFFICULTY_LEVEL.MEDIUM; }
        // low heartrate
        else { level = DIFFICULTY_LEVEL.EASY; }

        SelectDistraction(level);
    }

    private void SelectDistraction(DIFFICULTY_LEVEL level)
    {
        System.Random rand = new System.Random();
        string selectedDistraction = "null";
        int selectedNum;

        // need to shange string in dictionary to game object
        Dictionary<int, string> distractions = new Dictionary<int, string>() {
            { 1, "quiet_conversation" },
            { 2, "fan_buzzing" },
            { 3, "lights_flickering" },
            { 4, "alarm_clock" },
            { 5, "monitor_static" },
            { 6, "crowd_people" },
            { 7, "fire_alarm" },
            { 8, "earthquake" },
            { 9, "heartbeat" },
        };

        // select random task based on difficulty level
        switch (level)
        {
            case DIFFICULTY_LEVEL.EASY:
                // pick easy distraction
                // { queit conversation, fan buzzing, lights flickering }
                selectedNum = rand.Next(1, 3);
                selectedDistraction = distractions[selectedNum];
                break;
            case DIFFICULTY_LEVEL.MEDIUM:
                // pick medium distraction
                // { alarm clock, monitor static, crowd of people }
                selectedNum = rand.Next(4, 6);
                selectedDistraction = distractions[selectedNum];
                break;
            case DIFFICULTY_LEVEL.HARD:
                // pick hard distraction
                // { fire alarm, earthquake, heartbeat }
                selectedNum = rand.Next(7, 9);
                selectedDistraction = distractions[selectedNum];
                break;
        }

        SelectedDistration = selectedDistraction;
    }
}
