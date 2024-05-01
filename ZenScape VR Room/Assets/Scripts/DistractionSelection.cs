
using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DistractionSelection : MonoBehaviour
{
    public LivePulseListener listener;

    /*
        CALL listener.GetPublicPulse() and listener.GetPublicBaseline()
        to get public pulse and baseline return as ints
    */
    public TextMeshProUGUI watchText;
    public int Difficulty = 0;
    public float HeartRate;
    public string SelectedDistraction;
    [SerializeField] private ZenscapeTimer timer; 
    // low
    //public GameObject FlickeringLights;
    [SerializeField] private LightFlicker LightFlickerScript;
    //public GameObject QuietConversation;
    [SerializeField] private ConversationScript ConversationScript;
    //public GameObject FanBuzzing;
    [SerializeField] private FanBuzzingManager FanBuzzScript;
    // medium
    //public GameObject AlarmClock;
    [SerializeField] private AlarmClockDistraction AlarmClockScript;
    //public GameObject MonitorStatic;
    [SerializeField] private TVStaticManager TVStatic;
    //public GameObject Ambulance;
    [SerializeField] private RecordPlayerScript RecordPlayer;
    [SerializeField] private  AmbulanceDistraction AmbulanceScript;
    // high
    //public GameObject HeartBeat;
    [SerializeField] private HeartbeatDistraction HeartBeatScript;
    //public GameObject FireAlarm;
    [SerializeField] private FireAlarmManager FireAlarmScript;
    //public GameObject Earthquake;

    private float upperRate;
    private float lowerRate;
    private float baseline = 90;

    private enum DIFFICULTY_LEVEL
    {
        EASY,
        MEDIUM,
        HARD
    };
    // Can set all game objects here to call when selected

    void Start()
    {
        timer.TimeRemaining = 30;
        StartCoroutine(WaitForBaseline());
        StartCoroutine(GetPulseEverySecond());
    }

    IEnumerator WaitForBaseline()
    {
        // Wait until the baseline is fetched
        while (!listener.BaselineFetched)
        {
            yield return null;
        }
        // Once the baseline is fetched, set the baseline and other initialization logic
        baseline = listener.GetPublicBaseline();
        UnityEngine.Debug.Log("BASELINE:" + baseline);
        upperRate = baseline + 10;
        lowerRate = baseline - 10;
    }

    void Update()
    {
        // check ZenscapeTimer and call GetHeartRate() at certain times
        if (timer.TimeRemaining <= 0 && listener.BaselineFetched)
            GetHeartRate();
    }

    IEnumerator GetPulseEverySecond()
    {
        while (true)
        {
            try
            {
                // Get pulse data
                HeartRate = listener.GetPublicPulse();
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError("Error getting pulse data: " + e.Message);
            }

            // Wait for one second
            yield return new WaitForSeconds(1f);
        }
    }

    // select distraction
    public void GetHeartRate()
    {
        timer.TimeRemaining = 30;

        DIFFICULTY_LEVEL level = DIFFICULTY_LEVEL.EASY;
        
        // may need to change values after testing, these are temporary
        // high heartrate
        if (HeartRate >= upperRate) { 
            level = DIFFICULTY_LEVEL.EASY;  
            watchText.color =new Color32(255, 182, 193, 255);
        }
        // medium heartrate
        if (HeartRate < upperRate && HeartRate >  lowerRate){ 
            level = DIFFICULTY_LEVEL.MEDIUM; 
            watchText.color = Color.white;
        }
        // low heartrate
        if (HeartRate <= lowerRate) { 
            level = DIFFICULTY_LEVEL.HARD;
            watchText.color =new Color32(173, 216, 230, 255); 
        }

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
            { 6, "Record Player" },
            { 7, "Fire Alarm" },
            { 8, "Heart Beat" },
            { 9, "Ambulance" },
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
                // { alarm clock, monitor static, record player }
                selectedNum = rand.Next(4, 6);
                selectedDistraction = distractions[selectedNum];
                break;
            case DIFFICULTY_LEVEL.HARD:
                // pick hard distraction
                // { fire alarm, heartbeat, ambulance }
                selectedNum = rand.Next(7, 8);
                selectedDistraction = distractions[selectedNum];
                break;
        }

        StartDistraction(selectedDistraction);
    }

    private void StartDistraction(string distraction)
    {
        UnityEngine.Debug.Log($"The selected distraction is {distraction}");
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
            case "Record Player":
                RecordPlayer.StartEvent();
                break;
        }
    }
}
