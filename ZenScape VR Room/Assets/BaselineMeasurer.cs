using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using Newtonsoft.Json;
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
            Debug.LogError("VideoPlayer reference is not assigned!");
            return;
        }

        // Subscribe to the videoPlayer.loopPointReached event
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video Finished!");

        // Fetch active user's index from Firebase
        StartCoroutine(FetchActiveUserIndex());
    }

    IEnumerator FetchActiveUserIndex()
    {
        // Construct the URL for Firebase REST API to get active_user
        string activeUserUrl = firebaseURL + "/" + activeFirebaseRef + ".json";

        using (UnityWebRequest activeUserRequest = UnityWebRequest.Get(activeUserUrl))
        {
            yield return activeUserRequest.SendWebRequest();

            if (activeUserRequest.result == UnityWebRequest.Result.Success)
            {
                string activeUserJson = activeUserRequest.downloadHandler.text;

                if (!string.IsNullOrEmpty(activeUserJson))
                {
                    // Parse the activeUserJson to get active user's index
                    int activeUserIndex = ParseActiveUserIndex(activeUserJson);

                    if (activeUserIndex >= 0)
                    {
                        Debug.Log("Active User Index: " + activeUserIndex);

                        // Fetch zenscape_users array to get user's avg_pulse and pulse_history
                        string zenscapeUsersUrl = firebaseURL + "/" + loggedInFirebaseRef + ".json";

                        using (UnityWebRequest zenscapeUsersRequest = UnityWebRequest.Get(zenscapeUsersUrl))
                        {
                            yield return zenscapeUsersRequest.SendWebRequest();

                            if (zenscapeUsersRequest.result == UnityWebRequest.Result.Success)
                            {
                                string zenscapeUsersJson = zenscapeUsersRequest.downloadHandler.text;

                                // Update the user's baseline with avg_pulse
                                // Clear the user's pulse_history array
                                UpdateUserBaselineAndClearHistory(zenscapeUsersJson, activeUserIndex);
                            }
                            else
                            {
                                Debug.LogError("Error fetching zenscape_users: " + zenscapeUsersRequest.error);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Error parsing active user index from JSON.");
                    }
                }
                else
                {
                    Debug.LogError("Active user JSON is empty.");
                }
            }
            else
            {
                Debug.LogError("Error fetching active_user: " + activeUserRequest.error);
            }
        }
    }

    int ParseActiveUserIndex(string activeUserJson)
    {
        // Parse the activeUserJson to get active user's index
        // This assumes the JSON structure is something like: {"active_user": 0}
        // You may need to adjust this based on your actual Firebase data structure
        try
        {
            JObject jsonObject = JObject.Parse(activeUserJson);
            return jsonObject["active_user"].ToObject<int>();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parsing active user index from JSON: " + e.Message);
            return -1;
        }
    }

    void UpdateUserBaselineAndClearHistory(string zenscapeUsersJson, int activeUserIndex)
    {
        // Parse zenscapeUsersJson to find the active user's entry
        // This assumes zenscapeUsersJson is an array of user objects
        // You may need to adjust this based on your actual Firebase data structure

        try
        {
            JArray jsonArray = JArray.Parse(zenscapeUsersJson);

            if (activeUserIndex < jsonArray.Count)
            {
                JObject userObject = (JObject)jsonArray[activeUserIndex];

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
            }
            else
            {
                Debug.LogError("Active user index is out of range.");
            }
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
