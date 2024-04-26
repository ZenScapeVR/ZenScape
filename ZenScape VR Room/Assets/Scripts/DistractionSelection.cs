using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionSelection : MonoBehaviour
{
    public int Difficulty = 0;
    public float HeartRate = 89;
    public string SelectedDistraction;

    // low
    //public GameObject FlickeringLights;
    [SerializeField] private LightFlicker LightFlickerScript;
    //public GameObject QuietConversation;
    [SerializeField] private ConversationScript ConversationScript;
    //public GameObject FanBuzzing;
    [SerializeField] private AlarmClockDistraction FanBuzzScript;
    // medium
    //public GameObject AlarmClock;
    [SerializeField] private AlarmClockDistraction AlarmClockScript;
    //public GameObject MonitorStatic;
    [SerializeField] private TVStatic TVStatic;
    //public GameObject Ambulance;
    [SerializeField] private  AmbulanceDistraction AmbulanceScript;
    // high
    //public GameObject HeartBeat;
    [SerializeField] private HeartbeatDistraction HeartBeatScript;
    //public GameObject FireAlarm;
    [SerializeField] private FireAlarmScript FireAlarmScript;
    //public GameObject Earthquake;

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
        if (HeartRate >= 88 || Difficulty == 3) { level = DIFFICULTY_LEVEL.HARD; }
        // medium heartrate
        else if (HeartRate >=  74 || Difficulty == 2)  { level = DIFFICULTY_LEVEL.MEDIUM; }
        // low heartrate
        else { level = DIFFICULTY_LEVEL.EASY; }

        SelectDistraction(level);
    }

    private void SelectDistraction(DIFFICULTY_LEVEL level)
    {
        System.Random rand = new System.Random();
        string selectedDistraction = null;
        int selectedNum;

        // need to shange string in dictionary to game object
        Dictionary<int, string> distractions = new Dictionary<int, string>() {
            { 1, "Quiet Conversation" },
            { 2, "Flickering Lights" },
            { 3, "Fan Buzzing" },
            { 4, "Alarm Clock" },
            { 5, "Monitor Static" },
            { 6, "Ambulance" },
            { 7, "Fire Alarm" },
            { 8, "Heart Beat" },
            { 9, "Earthquake" },
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

        StartDistraction(selectedDistraction);
    }

    private void StartDistraction(string distraction)
    {
        Debug.Log($"The selected distraction is {distraction}");
        switch (distraction)
        {
            case "Flickering Lights":
                LightFlickerScript.StartEvent();
                break;
            case "Quiet Conversation":
                ConversationScript.StartEvent();
                break;
            case "Fan Buzzing":
                FanBuzzScript.StartEvent();
                break;
            case "Alarm Clock":
                AlarmClockScript.StartEvent();
                break;
            case "Monitor Static":
                TVStatic.StartEvent();
                break;
            case "Ambulance":
                AmbulanceScript.StartEvent();
                break;
            case "Fire Alarm":
                FireAlarmScript.StartEvent();
                break;
            case "Heart Beat":
                HeartBeatScript.StartEvent();
                break;
            case "Earthquake":
                Debug.Log("EARTHQUAKE - distraction not yet implemented");
                break;
        }
    }
}
