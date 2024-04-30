using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPlayerScript : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private AudioClip clip3;
    [SerializeField] private AudioClip clip4;
    [SerializeField] private AudioClip record_scratch;
    [SerializeField] private ZenscapeTimer timer;
    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        //StartEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying && timer.TimeRemaining <= 0)
            EndTask();
    }

    public void StartEvent()
    {
        isPlaying = true;
        System.Random rand = new System.Random();
        int clip = rand.Next(1, 4);
        switch (clip)
        {
            case 1:
                source.clip = clip1;
                source.Play();
                break;
            case 2:
                source.clip = clip2;
                source.Play();
                break;
            case 3:
                source.clip = clip3;
                source.Play();
                break;
            case 4:
                source.clip = clip4;
                source.Play();
                break;
        }
        timer.TimeRemaining = 15;
    }

    public void EndTask()
    {
        isPlaying = false;
        source.clip = record_scratch;
        source.Play();
        source.loop = false;
    }
}
