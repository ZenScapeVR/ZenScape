using UnityEngine;

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

    private void Start()
    {
        // Check if an AudioSource component is attached, if not, attach one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (ring != null)
        {
            // the phone is in a position to ring!!!
            PhoneRing();
        }
        else
        {
            Debug.LogError("Ring sound clip is not assigned!");
        }
    }

    private void Update()
    {
        if (isRinging && !hangUpCollider.bounds.Contains(transform.position))
        {
            StopPhoneRing();
        }
    }

    private void PhoneRing()
    {
        isRinging = true;
        audioSource.clip = ring;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void StopPhoneRing()
    {
        isRinging = false;
        audioSource.Stop();
        PickUpPhone();
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
        // Check if the object entering the trigger is the hangup collider
         Debug.Log("On trigger enter triggered");
        if (other == hangUpCollider)
        {
            // Handle the action for the phone being hung up
            HandleAction(true);
        }
    }

    public void HandleAction(bool hungUp)
    {
        Debug.Log("HandleAction called with hungUp: " + hungUp);

        if (isCallPickedUp)
        {
            if ((!isCallSpam && !hungUp) || (isCallSpam && hungUp))
            {
                if (audioSource != null)
                {
                    audioSource.clip = successSound;
                    audioSource.Play();
                    ResetPhone();
                }
            }
            else
            {
                if (audioSource != null)
                {
                    audioSource.clip = failureSound;
                    audioSource.Play();
                    ResetPhone();
                }
            }
        }
    }

    private void ResetPhone(){}
}
