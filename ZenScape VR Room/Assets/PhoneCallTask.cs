using System;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using System.Collections; // Add this line to use coroutines

public class PhoneCallTask : MonoBehaviour
{
    public AudioClip[] spamCallClips;
    public AudioClip[] realCallClips;
    public AudioClip successSound;
    public AudioClip failureSound;
    private bool isCallPickedUp;
    private bool isCallSpam;
    private AudioClip currentCallClip;
    private AudioSource audioSource;
    public AudioClip ring; // Assign your phone ringing sound clip in the Unity Editor
    public Collider hangUpCollider; // Assign your hang up collider in the Unity Editor
    private bool isRinging = true;
    private Vector3 originalPosition; 
    public AudioClip pickUpSound;
    public TextMeshPro phoneMetrics;
    private int attempts = 0;
    private int correct = 0;
    private float accuracy = 1.0f;
    private bool waitingForReturn = false;

    public PhoneTaskParent taskParent;

    private void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        StartTask();
    }

    public void StartTask(){
        transform.position = originalPosition;
        isRinging = true;
        isCallPickedUp = false;
        PhoneRing();
    }

    private void Update()
    {
        if (isRinging && !hangUpCollider.bounds.Contains(transform.position) && !isCallPickedUp)
        {
            StopPhoneRing();
        }
    }

    private void PhoneRing()
    {
        if(ring != null){
            SetPhoneMetrics();
            isRinging = true;
            audioSource.clip = ring;
            audioSource.loop = true;
            UnityEngine.Debug.LogError("PLAYING RING SOUND!");
            audioSource.Play();
        }else
        {
           UnityEngine.Debug.LogError("Ring sound clip is not assigned!");
        }
    }

    private void StopPhoneRing()
    {
        isRinging = false;
        audioSource.Stop();
        StartCoroutine(PlayAudioClipAndWait(pickUpSound, () => { PickUpPhone();}));
    }

    private void PickUpPhone()
    {
        isCallPickedUp = true;
        isCallSpam = UnityEngine.Random.value < 0.5f; // Randomly determine if the call is spam
        if (isCallSpam)
        {
            currentCallClip = spamCallClips[ UnityEngine.Random.Range(0, spamCallClips.Length)];
        }
        else
        {
            currentCallClip = realCallClips[ UnityEngine.Random.Range(0, realCallClips.Length)];
        }
        // Play the current call clip using the attached AudioSource
        audioSource.clip = currentCallClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
       UnityEngine.Debug.Log("On trigger enter triggered");
       if(!isRinging){
            if(waitingForReturn && other == hangUpCollider){
                StartCoroutine(PlayAudioClipAndWait(pickUpSound, () => { 
                    ResetPhone();
                    waitingForReturn = false;
                }));
            }
            else if (other == hangUpCollider)
            {
                StartCoroutine(PlayAudioClipAndWait(pickUpSound, () => { HandleAction(true);}));
            }
       }
    }

    public void HandleAction(bool hungUp)
    {
        if (isCallPickedUp)
        {
            attempts++;
            if ((!isCallSpam && !hungUp) || (isCallSpam && hungUp))
            {
                if (audioSource != null)
                {
                    correct++;
                    StartCoroutine(PlayAudioClipAndWait(successSound, () => {
                    SetPhoneMetrics();
                    if(hungUp){
                        ResetPhone();
                    }else{
                        waitingForReturn = true;
                    }
                    }));
                }
            }
            else
            {
                if (audioSource != null)
                {
                    StartCoroutine(PlayAudioClipAndWait(failureSound, () => {
                        SetPhoneMetrics();
                        ResetPhone();
                    }));
                }
            }
        }
    }

    private void ResetPhone()
    {
        Destroy(GetComponent<Rigidbody>());
        taskParent.EndGame(accuracy);
        Destroy(gameObject); // try to destroy the phone
    }
    
    private IEnumerator PlayAudioClipAndWait(AudioClip clip, System.Action callback)
    {
        audioSource.loop = false;
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length);
        callback?.Invoke();
    }


    private void SetPhoneMetrics()
    {
        if (attempts != 0)
        {
            accuracy = Mathf.Round(correct / attempts);
        }
        phoneMetrics.text = "Phone Call Task:\n" +
            "Answer calls as they come in before they go to voicemail! Hang up the phone if the call seems like spam, otherwise forward it with the green button.\n\n" +
            "Accuracy: " + accuracy.ToString("0.00"); // Format accuracy to display two decimal places
    }

}
