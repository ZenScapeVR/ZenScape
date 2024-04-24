using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationScript : MonoBehaviour
{
    [SerializeField] private GameObject conversation_location;
    [SerializeField] private ZenscapeTimer timer;
    [SerializeField] private bool event_active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.TimeRemaining == 0 && event_active)
            EndEvent();
    }

    void StartEvent()
    {
        event_active = true;
        conversation_location.GetComponent<AudioSource>().Play();
        timer.TimeRemaining = Random.Range(7, 16);
    }

    void EndEvent()
    {
        event_active = false;
        conversation_location.GetComponent<AudioSource>().Stop();
    }
}
