using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class BaselineMeasurer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign this in the Unity Editor
    public string firebaseURL = "https://zenscape-b6d91-default-rtdb.firebaseio.com/";
    public string activeFirebaseRef = "active_user";
    public string loggedInFirebaseRef = "zenscape_users";

    void Start()
    {
        if (videoPlayer == null)
        {
            UnityEngine.Debug.LogError("VideoPlayer reference is not assigned!");
            return;
        }

        // Subscribe to the videoPlayer.loopPointReached event
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        UnityEngine.Debug.Log("Video Finished!");

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
                UnityEngine.Debug.Log(activeUserJson);

                if (!string.IsNullOrEmpty(activeUserJson))
                {
                    // Parse the activeUserJson to get active user's userId
                    string activeUserId = ParseActiveUserId(activeUserJson);

                    if (!string.IsNullOrEmpty(activeUserId))
                    {
                        UnityEngine.Debug.Log("Active User Id: " + activeUserId);
                        // Fetch user's entry from zenscape_users
                        string zenscapeUsersUrl = firebaseURL + "/" + loggedInFirebaseRef + "/" + activeUserId + ".json";
                        using (UnityWebRequest zenscapeUsersRequest = UnityWebRequest.Get(zenscapeUsersUrl))
                        {
                            yield return zenscapeUsersRequest.SendWebRequest();

                            if (zenscapeUsersRequest.result == UnityWebRequest.Result.Success)
                            {
                                string zenscapeUserJson = zenscapeUsersRequest.downloadHandler.text;
                                UnityEngine.Debug.Log("Zenscape User Data: " + zenscapeUserJson);

                                // Update the user's baseline with avg_pulse and clear pulse_history
                                UpdateUserBaselineAndClearHistory(zenscapeUserJson, activeUserId);
                            }
                            else
                            {
                                UnityEngine.Debug.LogError("Error fetching zenscape_users: " + zenscapeUsersRequest.error);
                            }
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Active user's userId is invalid.");
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError("Active user JSON is empty.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Error fetching active_user: " + activeUserRequest.error);
            }
        }
    }

    string ParseActiveUserId(string activeUserJson)
    {
        try
        {
            // Parse the activeUserJson to get active user's userId
            JObject jsonObject = JObject.Parse(activeUserJson);
            return jsonObject["userId"]?.ToString();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Error parsing active user's userId from JSON: " + e.Message);
            return null;
        }
    }

    void UpdateUserBaselineAndClearHistory(string zenscapeUserJson, string activeUserId)
    {
        try
        {
            UnityEngine.Debug.Log(zenscapeUserJson);
            JObject userObject = JObject.Parse(zenscapeUserJson);
            if (userObject != null)
            {
                // Update baseline with avg_pulse
                if (userObject["avg_pulse"] != null)
                {
                    int avgPulse = (int)userObject["avg_pulse"];
                    userObject["baseline"] = avgPulse;

                    // Clear pulse_history array
                    userObject["pulse_history"] = "";

                    // Convert the updated JSON back to string
                    string updatedJsonData = userObject.ToString();

                    // Update zenscape_users in Firebase
                    string updateUrl = firebaseURL + "/" + loggedInFirebaseRef + "/" + activeUserId + ".json";
                    byte[] jsonDataBytes = System.Text.Encoding.UTF8.GetBytes(updatedJsonData);

                    StartCoroutine(UpdateUserData(updateUrl, jsonDataBytes));
                }
                else
                {
                    UnityEngine.Debug.LogError("avg_pulse is missing or invalid in zenscape user data.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Error parsing zenscape user data.");
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Error updating user data: " + e.Message);
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
                UnityEngine.Debug.LogError("Error updating zenscape_users: " + www.error);
            }
            else
            {
                UnityEngine.Debug.Log("Updated baseline and cleared pulse history for active user.");

                // Wait for 3 seconds before loading the "Lobby" scene
                yield return new WaitForSeconds(3f);

                // Load the "Lobby" scene
                SceneManager.LoadScene("Lobby");
            }
        }
    }
}
