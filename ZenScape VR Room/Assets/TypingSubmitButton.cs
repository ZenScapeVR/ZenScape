using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class TypingSubmitButton : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource audioSource;
    public AudioClip successClip;
    public AudioClip failureClip;

    private bool isPressed;

    public TextMeshPro textMesh; // Reference to the 3D TextMeshPro object
    public string firebaseURL = "https://zenscape-b6d91-default-rtdb.firebaseio.com/"; // Your Firebase URL
    public string activeFirebaseRef = "active_user"; // Firebase reference for "active"
    public string loggedInFirebaseRef = "zenscape_users"; // Firebase reference for "loggedIn"
    Vector3 originalPosition; // Store the original position of the button
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        isPressed = false;

        // Store the original position of the button
        originalPosition = button.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            isPressed = true;
            presser = other.gameObject;
            button.transform.Translate(Vector3.down * 0.015f); // Adjust the button press depth as needed
            onPress.Invoke();
            PlaySound(); // Play a click sound

            // Get the text from the TextMeshPro object
            string playerName = textMesh.text;

            // Check if playerName exists in Firebase
            StartCoroutine(CheckPlayerName(playerName));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPressed && other.gameObject == presser)
        {
            isPressed = false;
            ResetButtonPosition();
            onRelease.Invoke();
        }
    }

    private void ResetButtonPosition()
    {
        button.transform.position = originalPosition;
    }

    private void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(successClip); // Play a click sound
        }
    }

    private void PlayFailureSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(failureClip); // Play a failure sound
        }
    }

    IEnumerator CheckPlayerName(string playerName)
    {
        // Construct the URL for Firebase REST API
        string url = firebaseURL + "/" + loggedInFirebaseRef + "/" + playerName + ".json";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string result = www.downloadHandler.text;

                if (result == "null" || playerName == "")
                {
                    // Player does not exist
                    Debug.Log("Player does not exist!");

                    // Play failure sound
                    PlayFailureSound();
                }
                else
                {
                    // Player exists
                    Debug.Log("Player exists!");
                    // Play success sound
                    PlaySound();
                    // Post playerName to active Firebase reference
                    StartCoroutine(PostToFirebase(playerName));
                    // Change to "Lobby" scene
                    SceneManager.LoadScene("Lobby");
                }
            }
            else
            {
                // Error in web request
                Debug.Log("Error checking player name: " + www.error);

                // Play failure sound
                PlayFailureSound();
            }
        }
    }

    IEnumerator PostToFirebase(string playerName)
    {
        // Construct the URL for Firebase REST API
        string url = firebaseURL + "/" + activeFirebaseRef + ".json";

        // Construct the JSON data
        string jsonData = "{\"playerName\": \"" + playerName + "\"}";

        byte[] jsonDataBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = UnityWebRequest.Put(url, jsonDataBytes))
        {
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error posting to Firebase: " + www.error);
            }
            else
            {
                Debug.Log("Posted playerName to Firebase.");
            }
        }
    }
}
