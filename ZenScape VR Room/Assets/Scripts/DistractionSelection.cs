using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionSelection : MonoBehaviour
{
    public int Difficulty = 0;
    public float HearRate = 0f;
    public GameObject SelectedDistraction;

    // low
    public GameObject FlickeringLights;
    public GameObject QuietConversation;
    public GameObject FanBuzzing;
    // medium
    public GameObject AlarmClock;
    public GameObject MonitorStatic;
    public GameObject Crowd;
    // high
    public GameObject HeartBeat;
    public GameObject FireAlarm;
    public GameObject Earthquake;

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
        GameObject selectedDistraction = null;
        int selectedNum;

        // need to shange string in dictionary to game object
        Dictionary<int, GameObject> distractions = new Dictionary<int, GameObject>() {
            { 1, QuietConversation },
            { 2, FlickeringLights },
            { 3, FanBuzzing },
            { 4, AlarmClock },
            { 5, MonitorStatic },
            { 6, Crowd },
            { 7, FireAlarm },
            { 8, HeartBeat },
            { 9, Earthquake },
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

        SelectedDistraction = selectedDistraction;
    }
}
