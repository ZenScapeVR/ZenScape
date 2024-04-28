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
    private bool waitingForReturn = false;

    /* right now, this task triggers every 30 seconds. */

    private void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }


        StartCoroutine(ScheduleTask(30f));
    }

    IEnumerator ScheduleTask(float seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds); // Wait for 30 seconds
            StartTask(); // Call the StartTask method
        }
    }

    public void StartTask(){
        transform.position = originalPosition;
        isRinging = true;
        isCallPickedUp = false;
        if (!isCallPickedUp)  // Only start ringing if the phone is not picked up
        {
            PhoneRing();
        }
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
        if(ring != null && !isCallPickedUp){
            SetPhoneMetrics();
            isRinging = true;
            audioSource.clip = ring;
            audioSource.loop = true;
            audioSource.Play();
        }else
        {
            Debug.LogError("Ring sound clip is not assigned!");
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
        isCallSpam = Random.value < 0.5f; // Randomly determine if the call is spam

        if (isCallSpam)
        {
            currentCallClip = spamCallClips[Random.Range(0, spamCallClips.Length)];
        }
        else
        {
            currentCallClip = realCallClips[Random.Range(0, realCallClips.Length)];
        }
        
        // Play the current call clip using the attached AudioSource
        audioSource.clip = currentCallClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("On trigger enter triggered");
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
        transform.position = originalPosition;
        isRinging = true;
        isCallPickedUp = false;
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
        float accuracy = 1.0f;
        if (attempts != 0)
        {
            accuracy = (correct / attempts);
        }
        accuracy = Mathf.Round(accuracy * 100);
        phoneMetrics.text = "Phone Call Task:\n" +
            "Answer calls as they come in before they go to voicemail! Hang up the phone if the call seems like spam, otherwise forward it with the green button.\n\n" +
            "Accuracy: " + accuracy.ToString("0.00"); // Format accuracy to display two decimal places
    }

}
