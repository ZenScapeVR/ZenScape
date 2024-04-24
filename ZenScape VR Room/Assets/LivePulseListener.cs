using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;

public class LivePulseListener : MonoBehaviour
{
    // URL of the API endpoint
    private string firebaseURL = "https://zenscape-b6d91-default-rtdb.firebaseio.com/";
    private string activeFirebaseRef = "active_user";
    private string loggedInFirebaseRef = "zenscape_users";

    // Reference to the TextMeshPro component
    public TextMeshProUGUI textMeshPro;

    // Frequency of API requests (in seconds)
    public float requestInterval = 1f; // Update every 1 second

    private string activeUserId;

    // Start is called before the first frame update
    void Start()
    {
        // Start coroutine to fetch active user data
        StartCoroutine(FetchActiveUserData());
        
        // Start coroutine to continuously fetch live pulse data
        StartCoroutine(FetchLivePulseData());
    }

    // Coroutine to fetch active user data once
    IEnumerator FetchActiveUserData()
    {
        // Construct the URL to get active user's data
        string activeUserUrl = firebaseURL + "/" + activeFirebaseRef + ".json";
        using (UnityWebRequest activeUserRequest = UnityWebRequest.Get(activeUserUrl))
        {
            yield return activeUserRequest.SendWebRequest();

            if (activeUserRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching active_user: " + activeUserRequest.error);
                yield break;
            }

            string activeUserJson = activeUserRequest.downloadHandler.text;
            activeUserId = ParseActiveUserId(activeUserJson);

            if (string.IsNullOrEmpty(activeUserId))
            {
                Debug.LogError("Active user's userId is invalid.");
                yield break;
            }

            Debug.Log("Active User Id: " + activeUserId);
        }
    }

    // Coroutine to continuously fetch live pulse data
    IEnumerator FetchLivePulseData()
    {
        while (true)
        {
            if (!string.IsNullOrEmpty(activeUserId))
            {
                // Construct the URL to get user's data by userId
                string userUrl = firebaseURL + "/" + loggedInFirebaseRef + "/" + activeUserId + ".json";
                using (UnityWebRequest userRequest = UnityWebRequest.Get(userUrl))
                {
                    yield return userRequest.SendWebRequest();

                    if (userRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Error fetching user data: " + userRequest.error);
                    }
                    else
                    {
                        string userDataJson = userRequest.downloadHandler.text;
                        int livePulse = GetLivePulseFromUserData(userDataJson);

                        // Set the text of the TextMeshPro component to the live pulse value
                        textMeshPro.text = livePulse.ToString();

                        // Log the live pulse value
                        Debug.Log("Live Pulse: " + livePulse);
                    }
                }
            }

            // Wait for specified interval before fetching live pulse data again
            yield return new WaitForSeconds(requestInterval);
        }
    }

    // Method to parse active user's userId from JSON using Newtonsoft.Json
    string ParseActiveUserId(string activeUserJson)
    {
        try
        {
            dynamic jsonObject = JsonConvert.DeserializeObject(activeUserJson);
            return jsonObject["userId"];
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parsing active user's userId from JSON: " + e.Message);
            return null;
        }
    }

    // Method to extract live pulse from user data JSON using Newtonsoft.Json
    int GetLivePulseFromUserData(string userDataJson)
    {
        try
        {
            dynamic userData = JsonConvert.DeserializeObject(userDataJson);
            return userData["live_pulse"];
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error extracting live pulse from user data JSON: " + e.Message);
            return 0;
        }
    }
}
