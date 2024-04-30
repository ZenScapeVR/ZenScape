using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class DayManager : MonoBehaviour
{
    // Start is called before the first frame update
    // At the beginning of every day, we need to clear the pulse history and avg_pulse.
    // At the end of the day, we need to push the ACTIVE PULSE
    public int day = 0;
    public string firebaseURL = "https://zenscape-b6d91-default-rtdb.firebaseio.com/";
    public string activeFirebaseRef = "active_user";
    public string loggedInFirebaseRef = "zenscape_users";
    private ActiveUserData activeUser;
    public LobbyPortal portal;

    [System.Serializable]
    public class ActiveUserData
    {
        public string userId;
    }

    [System.Serializable]
    public class UserData
    {
        public int live_pulse;
        public int avg_pulse;
        public int pulse_history;
    }

    void Start()
    {
        StartCoroutine(ClearUserPulseHistory());    // At the beginning of every day, we need to clear the pulse history and avg_pulse.
    }

    // Start is called before the first frame update
    // At the beginning of every day, we need to clear the pulse history and avg_pulse.
    // At the end of the day, we need to push the ACTIVE PULSE

    public void EndDayDatabaseUpdate(float phone, float coffee, float sort){
        StartCoroutine(EndDayRoutine(phone, coffee, sort));
        portal.EnablePortal();
    }

    IEnumerator EndDayRoutine(float phone, float coffee, float sort)
    {
        UnityEngine.Debug.Log("Adding metrics to firebase with coffee: " + coffee + " phone: " + phone + " and sort: " + sort);
        string activeUserId = activeUser.userId;
        if (!string.IsNullOrEmpty(activeUserId))
        {
            // Fetch user's data from zenscape_users
            string zenscapeUsersUrl = firebaseURL + "/" + loggedInFirebaseRef + "/" + activeUserId + ".json";
            using (UnityWebRequest zenscapeUsersRequest = UnityWebRequest.Get(zenscapeUsersUrl))
            {
                yield return zenscapeUsersRequest.SendWebRequest();

                if (zenscapeUsersRequest.result == UnityWebRequest.Result.Success)
                {
                    string zenscapeUserJson = zenscapeUsersRequest.downloadHandler.text;
                    JObject userObject = JObject.Parse(zenscapeUserJson);

                    if (userObject != null)
                    {
                        // Retrieve average pulse from user's data
                        int avgPulse = userObject["avg_pulse"] != null ? userObject["avg_pulse"].Value<int>() : 0;
                        // Update user's data for the current day
                        userObject["metrics"][day]["avg_pulse"] = avgPulse;
                        // add coffee accuracy 
                        userObject["metrics"][day]["coffee"] = coffee;
                        // add phone accuracy 
                        userObject["metrics"][day]["phone"] = phone;
                        // add sort accuracy 
                        userObject["metrics"][day]["sort"] = sort;
                        // add overall accuracy 
                        userObject["metrics"][day]["overall"] = (sort + coffee + phone) / 3;

                        // Convert the updated JSON back to string
                        string updatedJsonData = userObject.ToString();
                        // Update user's data in Firebase
                        string updateUrl = firebaseURL + "/" + loggedInFirebaseRef + "/" + activeUserId + ".json";
                        byte[] jsonDataBytes = System.Text.Encoding.UTF8.GetBytes(updatedJsonData);
                        StartCoroutine(UpdateUserData(updateUrl, jsonDataBytes));
                    }
                    else
                    {
                        Debug.LogError("Error parsing zenscape user data.");
                    }
                }
                else
                {
                    Debug.LogError("Error fetching zenscape_users: " + zenscapeUsersRequest.error);
                }
            }
        }
        else
        {
            Debug.LogError("Active user's userId is invalid.");
        }
    }

    IEnumerator ClearUserPulseHistory()
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
                                ClearHistory(zenscapeUserJson, activeUserId);
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
            ActiveUserData jsonObject = JsonConvert.DeserializeObject<ActiveUserData>(activeUserJson);
            activeUser = jsonObject;
            return jsonObject.userId;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parsing active user's userId from JSON: " + e.Message);
            return null;
        }
    }

    void ClearHistory(string zenscapeUserJson, string activeUserId)
    {
        try
        {
            UnityEngine.Debug.Log(zenscapeUserJson);
            JObject userObject = JObject.Parse(zenscapeUserJson);
            if (userObject != null)
            {
                // Clear pulse_history array
                userObject["pulse_history"] = "0";
                userObject["avg_pulse"] = "0";
                // Convert the updated JSON back to string
                string updatedJsonData = userObject.ToString();
                // Update zenscape_users in Firebase
                string updateUrl = firebaseURL + "/" + loggedInFirebaseRef + "/" + activeUserId + ".json";
                byte[] jsonDataBytes = System.Text.Encoding.UTF8.GetBytes(updatedJsonData);
                StartCoroutine(UpdateUserData(updateUrl, jsonDataBytes));
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
                UnityEngine.Debug.Log("Cleared pulse history and avg_pulse for active user.");
            }
        }
    }

}
