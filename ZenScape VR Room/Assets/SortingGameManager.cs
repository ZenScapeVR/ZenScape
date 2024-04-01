using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SortingGameManager : MonoBehaviour
{
    public GameObject sortingObjectsContainer; // Reference to the Sorting Objects game object
    public GameObject sortingObjectsPrefab; // Array of sorting object prefabs to respawn
    public AudioClip gameEndSound; // Sound to play when the game ends
    private AudioSource audioSource;
    private int sortingObjectsRemaining;
    private int correct;
    private int incorrect;
    private int attempts;
    private float accuracy = 1f;
    public TextMeshPro metrics;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Initialize the number of sorting objects remaining
        if (sortingObjectsContainer != null)
        {
            sortingObjectsRemaining = sortingObjectsContainer.transform.childCount;
        }
    }

    // Called when a bad sort is made
    public void BadSort()
    {
        incorrect++;
        attempts++;
        UpdateMetrics();
    }

    // Called when a good sort is made
    public void GoodSort()
    {
        correct++;
        attempts++;
        UpdateMetrics();
    }

    // Update metrics text
    public void UpdateMetrics()
    {
        // Calculate accuracy
        if (attempts != 0)
        {
            accuracy = (float)correct / (float)attempts;
        }
        else
        {
            accuracy = 1f;
        }

        // Update UI text
        metrics.text = "Correct: " + correct + "\nIncorrect: " + incorrect + "\nAccuracy: " + accuracy.ToString("P0");
    }


    // Function to be called when a sorting object is destroyed
    public void SortingObjectDestroyed()
    {
        sortingObjectsRemaining--;

        // Check if all sorting objects have been destroyed
        if (sortingObjectsRemaining <= 0)
        {
            // Game end actions
            EndGame();
        }
    }

    void EndGame()
    {
        // Play game end sound
        correct = 0;
        incorrect = 0;
        attempts = 0;
        accuracy = 1f;
        UpdateMetrics();
        PlaySound(gameEndSound);
        // Instantiate a new instance of sortingObjectsPrefab
        GameObject newSortingObjects = Instantiate(sortingObjectsPrefab, sortingObjectsContainer.transform);

        // Update sortingObjectsContainer and sortingObjectsRemaining
        sortingObjectsContainer = newSortingObjects;
        sortingObjectsRemaining = sortingObjectsContainer.transform.childCount;
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
