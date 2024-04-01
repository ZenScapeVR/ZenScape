using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingBin : MonoBehaviour
{
    public string binName;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    private AudioSource audioSource;

    // Reference to the SortingGameManager script
    public SortingGameManager sortingGameManager;

    void Start()
    {
        // Set binName to the name of the GameObject
        binName = gameObject.name;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Sortable sortable = other.GetComponent<Sortable>();
        if (sortable != null && sortable.ChosenName == binName)
        {
            // Provide feedback for correct sorting (e.g., destroy the sorting object)
            Destroy(other.gameObject);
            PlaySound(correctSound);
            // Call SortingObjectDestroyed method from SortingGameManager script
            if (sortingGameManager != null)
            {
                sortingGameManager.GoodSort();
                sortingGameManager.SortingObjectDestroyed();
            }
            // You can add more feedback here, like scoring, particle effects, etc.
            Debug.Log("Sorting object matched with bin: " + binName);
        }
        else if (sortable != null)
        {
            // Provide feedback for incorrect sorting
            PlaySound(incorrectSound);
            sortable.ResetPosition();
            if (sortingGameManager != null)
            {
                sortingGameManager.BadSort();
                Debug.Log("Incorrect sorting for object: " + (sortable != null ? sortable.ChosenName : "Unknown"));
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}