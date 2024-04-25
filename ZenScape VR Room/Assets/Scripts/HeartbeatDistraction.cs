using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatDistraction : MonoBehaviour
{
    [SerializeField] private AudioSource head;
    [SerializeField] private AudioClip heartbeat;
    public DistractionSelection distraction_script;
    private float heartrate;
    [SerializeField] private ZenscapeTimer timer;
    private bool is_active = false;
    // Start is called before the first frame update
    void Start()
    {
        //StartEvent();
    }

    // Update is called once per frame
    void Update()
    {
        heartrate = distraction_script.HeartRate;
        if (is_active && timer.TimeRemaining <= 0)
            EndEvent();
    }

    void StartEvent()
    {
        is_active = true;
        head.GetComponent<AudioSource>().loop = true;
        PlaySound(heartbeat);
        timer.TimeRemaining = Random.Range(10, 20);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && head != null)
        {
            head.clip = clip;
            head.Play();
        }
    }

    void EndEvent()
    {
        is_active = false;
        head.Stop();
        head.GetComponent<AudioSource>().loop = false;
    }
}
