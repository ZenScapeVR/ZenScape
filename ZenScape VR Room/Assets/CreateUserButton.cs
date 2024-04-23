using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[System.Serializable]
public class UserData
{
    public Dictionary<string, UserEntry> entries;
}

[System.Serializable]
public class UserEntry
{
    public string playerName;
    public int userId;
}

public class CreateUserButton : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource audioSource;
    public AudioClip successClip;
    public AudioClip failureClip;

    private bool isPressed;

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

            // Add a new user
            StartCoroutine(AddNewUser());
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

    IEnumerator AddNewUser()
    {
        // Construct the URL for Firebase REST API
        string loggedInUrl = firebaseURL + "/" + loggedInFirebaseRef + ".json";
        string activeUrl = firebaseURL + "/" + activeFirebaseRef + ".json";

        using (UnityWebRequest loggedInRequest = UnityWebRequest.Get(loggedInUrl))
        {
            yield return loggedInRequest.SendWebRequest();

            if (loggedInRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = loggedInRequest.downloadHandler.text;

                // Deserialize the JSON response to get existing user IDs
                UserData userData = JsonUtility.FromJson<UserData>(jsonResponse);

                if (userData != null && userData.entries != null)
                {
                    List<int> existingUserIds = ParseExistingUserIds(userData);

                    // Find the lowest available user ID that doesn't already exist
                    int newUserId = FindLowestAvailableUserId(existingUserIds);

                    // Add the new user to loggedIn Firebase
                    UserEntry newEntry = new UserEntry
                    {
                        playerName = newUserId.ToString(),
                        userId = newUserId
                    };

                    userData.entries.Add(newUserId.ToString(), newEntry);
                    string updatedJsonData = JsonUtility.ToJson(userData);

                    using (UnityWebRequest loggedInPostRequest = UnityWebRequest.Put(loggedInUrl, updatedJsonData))
                    {
                        loggedInPostRequest.SetRequestHeader("Content-Type", "application/json");
                        yield return loggedInPostRequest.SendWebRequest();

                        if (loggedInPostRequest.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log("Error adding new user to loggedIn: " + loggedInPostRequest.error);
                            PlayFailureSound();
                            yield break;
                        }
                    }

                    // Add the new user ID as playerName to active Firebase
                    string activeJsonData = "{\"playerName\": \"" + newUserId.ToString() + "\", \"userId\": " + newUserId.ToString() + "}";

                    using (UnityWebRequest activePostRequest = UnityWebRequest.Put(activeUrl, activeJsonData))
                    {
                        activePostRequest.SetRequestHeader("Content-Type", "application/json");
                        yield return activePostRequest.SendWebRequest();

                        if (activePostRequest.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log("Error adding new user to active: " + activePostRequest.error);
                            PlayFailureSound();
                            yield break;
                        }
                    }

                    Debug.Log("Added new user with ID: " + newUserId);
                    PlaySound();
                }
                else
                {
                    Debug.Log("Error parsing JSON response for existing user IDs.");
                    PlayFailureSound();
                }
            }
            else
            {
                Debug.Log("Error getting existing user IDs: " + loggedInRequest.error);
                PlayFailureSound();
            }
        }
    }

    private List<int> ParseExistingUserIds(UserData userData)
    {
        List<int> existingUserIds = new List<int>();

        if (userData != null && userData.entries != null)
        {
            foreach (var entry in userData.entries.Values)
            {
                existingUserIds.Add(entry.userId);
            }
        }

        return existingUserIds;
    }

    private int FindLowestAvailableUserId(List<int> existingUserIds)
    {
        int newUserId = 1; // Start from 1

        while (existingUserIds.Contains(newUserId))
        {
            newUserId++;
        }

        return newUserId;
    }
}
