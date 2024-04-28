using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBuzzingManager : MonoBehaviour
{
    [SerializeField] private AlarmClockDistraction fan1;
    [SerializeField] private AlarmClockDistraction fan2;
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
        fan1.StartEvent();
        fan2.StartEvent();
    }
}
