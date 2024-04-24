using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine.SceneManagement;
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
                // Use JSON.NET to parse the JSON array
                JArray jsonArray = JArray.Parse(jsonResponse);
                // Add a new user entry
                JObject newUser = new JObject();
                newUser["avg_pulse"] = "100"; // Example data
                newUser["baseline"] = 80; // Example data
                newUser["live_pulse"] = "90"; // Example data

                JArray metricsArray = new JArray();
                JObject metric1 = new JObject();
                metric1["accuracy"] = 95; // Example data
                metric1["avg_pulse"] = 85; // Example data
                metricsArray.Add(metric1);
                // Add more metrics as needed

                newUser["metrics"] = metricsArray;

                jsonArray.Add(newUser);

                string updatedJsonData = jsonArray.ToString();

                // Update the data on Firebase
                using (UnityWebRequest loggedInPostRequest = UnityWebRequest.Put(loggedInUrl, updatedJsonData))
                {
                    loggedInPostRequest.SetRequestHeader("Content-Type", "application/json");
                    yield return loggedInPostRequest.SendWebRequest();

                    if (loggedInPostRequest.result != UnityWebRequest.Result.Success)
                    {
                        UnityEngine.Debug.Log("Error adding new user: " + loggedInPostRequest.error);
                        PlayFailureSound();
                        yield break;
                    }
                }

                UnityEngine.Debug.Log("Added new user");
                PlaySound();
                StartCoroutine(SetActiveUser(jsonArray.Count - 1));
            }
            else
            {
                UnityEngine.Debug.Log("Error getting existing users: " + loggedInRequest.error);
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
                UnityEngine.Debug.Log("Set active user to ID: " + userId);
                yield return new WaitForSeconds(1.0f); // Adjust the delay as needed
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                UnityEngine.Debug.Log("Error setting active user: " + activePostRequest.error);
            }
        }
    }
}
