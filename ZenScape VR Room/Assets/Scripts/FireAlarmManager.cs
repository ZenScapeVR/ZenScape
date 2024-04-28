using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireAlarmManager : MonoBehaviour
{
    [SerializeField] private FireAlarmScript alarm1;
    [SerializeField] private FireAlarmScript alarm2;
    [SerializeField] private FireAlarmScript alarm3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartEvent()
    {
        alarm1.StartEvent();
        alarm2.StartEvent();
        alarm3.StartEvent();
    }
}
