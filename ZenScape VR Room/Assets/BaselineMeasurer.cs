using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class TutorialVideoScript : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign this in the Unity Editor
    public string firebaseURL = "https://zenscape-b6d91-default-rtdb.firebaseio.com/";
    public string activeFirebaseRef = "active_user";
    public string loggedInFirebaseRef = "zenscape_users";

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer reference is not assigned!");
            return;
        }

        // Subscribe to the videoPlayer.loopPointReached event
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video Finished!");

        // Fetch active user's userId from Firebase
        StartCoroutine(FetchActiveUserId());
    }

    IEnumerator FetchActiveUserId()
    {
        // Construct the URL for Firebase REST API to get active_user's userId
        string activeUserUrl = firebaseURL + "/" + activeFirebaseRef + ".json";

        using (UnityWebRequest activeUserRequest = UnityWebRequest.Get(activeUserUrl))
        {
            yield return activeUserRequest.SendWebRequest();

            if (activeUserRequest.result == UnityWebRequest.Result.Success)
            {
                string activeUserJson = activeUserRequest.downloadHandler.text;

                if (!string.IsNullOrEmpty(activeUserJson))
                {
                    // Parse the activeUserJson to get active user's userId
                    string activeUserId = ParseActiveUserId(activeUserJson);

                    if (!string.IsNullOrEmpty(activeUserId))
                    {
                        Debug.Log("Active User Id: " + activeUserId);
                        // Fetch zenscape_users array to find the user's entry
                        string zenscapeUsersUrl = firebaseURL + "/" + loggedInFirebaseRef + ".json";

                        using (UnityWebRequest zenscapeUsersRequest = UnityWebRequest.Get(zenscapeUsersUrl))
                        {
                            yield return zenscapeUsersRequest.SendWebRequest();

                            if (zenscapeUsersRequest.result == UnityWebRequest.Result.Success)
                            {
                                string zenscapeUsersJson = zenscapeUsersRequest.downloadHandler.text;

                                // Update the user's baseline with avg_pulse
                                // Clear the user's pulse_history array
                                UpdateUserBaselineAndClearHistory(zenscapeUsersJson, activeUserId);
                            }
                            else
                            {
                                Debug.Log("Error fetching zenscape_users: " + zenscapeUsersRequest.error);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Active user's userId is invalid.");
                    }
                }
                else
                {
                    Debug.Log("Active user JSON is empty.");
                }
            }
            else
            {
                Debug.Log("Error fetching active_user: " + activeUserRequest.error);
            }
        }
    }

    string ParseActiveUserId(string activeUserJson)
    {
        try
        {
            // Parse the activeUserJson to get active user's userId
            Debug.Log(activeUserJson);
            JObject jsonObject = JObject.Parse(activeUserJson);
            return jsonObject["userId"].ToString();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parsing active user's userId from JSON: " + e.Message);
            return null;
        }
    }

    void UpdateUserBaselineAndClearHistory(string zenscapeUsersJson, string activeUserId)
    {
        try
        {
            // Parse zenscapeUsersJson to find the active user's entry
            JArray jsonArray = JArray.Parse(zenscapeUsersJson);

            foreach (JObject userObject in jsonArray)
            {
                if (userObject != null && userObject["avg_pulse"] != null && userObject["avg_pulse"].ToString() == activeUserId)
                {
                    // Found the user, update baseline with avg_pulse
                    int avgPulse = (int)userObject["avg_pulse"];
                    userObject["baseline"] = avgPulse;

                    // Clear pulse_history array
                    userObject["pulse_history"] = new JArray();

                    // Convert the updated JSON back to string
                    string updatedJsonData = jsonArray.ToString();

                    // Update zenscape_users in Firebase
                    string updateUrl = firebaseURL + "/" + loggedInFirebaseRef + ".json";

                    byte[] jsonDataBytes = System.Text.Encoding.UTF8.GetBytes(updatedJsonData);

                    StartCoroutine(UpdateUserData(updateUrl, jsonDataBytes));
                    return; // Exit loop once user is found and updated
                }
            }

            // If active user is not found
            Debug.LogError("Active user with userId " + activeUserId + " not found.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error updating user data: " + e.Message);
        }
    }

    IEnumerator UpdateUserData(string updateUrl, byte[] jsonDataBytes)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(updateUrl, jsonDataBytes))
        {
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error updating zenscape_users: " + www.error);
            }
            else
            {
                Debug.Log("Updated baseline and cleared pulse history for active user.");

                // Wait for 3 seconds before loading the "Lobby" scene
                yield return new WaitForSeconds(3f);

                // Load the "Lobby" scene
                SceneManager.LoadScene("Lobby");
            }
        }
    }
}
