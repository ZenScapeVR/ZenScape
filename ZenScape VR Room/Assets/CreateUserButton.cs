using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[System.Serializable]
public class UserData
{
    public List<UserEntry> zenscape_users;
}

[System.Serializable]
public class UserEntry
{
    public string avg_pulse;
    public int baseline;
    public string live_pulse;
    public Metrics[] metrics;
    public string pulse_history;
}

[System.Serializable]
public class Metrics
{
    public int accuracy;
    public int avg_pulse;
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
    public string loggedInFirebaseRef = "zenscape_users"; // Firebase reference for "loggedIn"
    public string activeFirebaseRef = "active_user"; // Firebase reference for "active_user"

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

        using (UnityWebRequest loggedInRequest = UnityWebRequest.Get(loggedInUrl))
        {
            yield return loggedInRequest.SendWebRequest();

            if (loggedInRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = loggedInRequest.downloadHandler.text;

                // Deserialize the JSON response to get existing user entries
                UserData userData = JsonUtility.FromJson<UserData>("{\"zenscape_users\":" + jsonResponse + "}");

                if (userData != null)
                {
                    // Add a new user entry
                    userData.zenscape_users.Add(new UserEntry());

                    // Serialize the updated data
                    string updatedJsonData = "{\"zenscape_users\":" + JsonHelper.ToJson(userData.zenscape_users) + "}";

                    // Update the data on Firebase
                    using (UnityWebRequest loggedInPostRequest = UnityWebRequest.Put(loggedInUrl, updatedJsonData))
                    {
                        loggedInPostRequest.SetRequestHeader("Content-Type", "application/json");
                        yield return loggedInPostRequest.SendWebRequest();

                        if (loggedInPostRequest.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log("Error adding new user: " + loggedInPostRequest.error);
                            PlayFailureSound();
                            yield break;
                        }
                    }

                    Debug.Log("Added new user");
                    PlaySound();

                    // Now set active_user to the new userId
                    StartCoroutine(SetActiveUser(userData.zenscape_users.Count - 1));
                }
                else
                {
                    Debug.Log("Error parsing JSON response for existing users.");
                    PlayFailureSound();
                }
            }
            else
            {
                Debug.Log("Error getting existing users: " + loggedInRequest.error);
                PlayFailureSound();
            }
        }
    }

    IEnumerator SetActiveUser(int userId)
    {
        string activeUrl = firebaseURL + "/" + activeFirebaseRef + ".json";
        string activeJsonData = "{\"userId\": \"" + userId.ToString() + "\"}";

        using (UnityWebRequest activePostRequest = UnityWebRequest.Put(activeUrl, activeJsonData))
        {
            activePostRequest.SetRequestHeader("Content-Type", "application/json");
            yield return activePostRequest.SendWebRequest();

            if (activePostRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Set active user to ID: " + userId);
            }
            else
            {
                Debug.Log("Error setting active user: " + activePostRequest.error);
            }
        }
    }
}

public static class JsonHelper
{
    // Helper class to serialize and deserialize JSON arrays
    public static string ToJson<T>(List<T> list)
    {
        Wrapper<T> wrapper = new Wrapper<T>
        {
            Items = list
        };
        return JsonUtility.ToJson(wrapper);
    }

    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}
