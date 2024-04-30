using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // Add this line to import JToken
public class UserMetricsDisplay : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public string firebaseURL = "https://zenscape-b6d91-default-rtdb.firebaseio.com/";
    public string activeFirebaseRef = "active_user";
    public string loggedInFirebaseRef = "zenscape_users";

    [System.Serializable]
    public class ActiveUserData
    {
        public string userId;
    }


    void Start()
    {
        StartCoroutine(FetchActiveUserData());
    }

    IEnumerator FetchActiveUserData()
    {
        // Construct the URL for Firebase REST API to get active_user's userId
        string activeUserUrl = firebaseURL + "/" + activeFirebaseRef + ".json";
        using (UnityWebRequest activeUserRequest = UnityWebRequest.Get(activeUserUrl))
        {
            yield return activeUserRequest.SendWebRequest();

            if (activeUserRequest.result == UnityWebRequest.Result.Success)
            {
                string activeUserJson = activeUserRequest.downloadHandler.text;
                string activeUserId = ParseActiveUserId(activeUserJson);
                
                if (!string.IsNullOrEmpty(activeUserId))
                {
                    // Fetch user's entry from zenscape_users
                    string zenscapeUserUrl = firebaseURL + "/" + loggedInFirebaseRef + "/" + activeUserId + ".json";
                    using (UnityWebRequest zenscapeUserRequest = UnityWebRequest.Get(zenscapeUserUrl))
                    {
                        yield return zenscapeUserRequest.SendWebRequest();

                        if (zenscapeUserRequest.result == UnityWebRequest.Result.Success)
                        {
                            string zenscapeUserJson = zenscapeUserRequest.downloadHandler.text;
                            UpdateDisplay(zenscapeUserJson);
                        }
                        else
                        {
                            Debug.LogError("Error fetching zenscape_user data: " + zenscapeUserRequest.error);
                        }
                    }
                }
                else
                {
                    Debug.LogError("Active user's userId is invalid.");
                }
            }
            else
            {
                Debug.LogError("Error fetching active_user: " + activeUserRequest.error);
            }
        }
    }

    string ParseActiveUserId(string activeUserJson)
    {
        try
        {
            ActiveUserData jsonObject = JsonConvert.DeserializeObject<ActiveUserData>(activeUserJson);
            return jsonObject.userId;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parsing active user's userId from JSON: " + e.Message);
            return null;
        }
    }

    void UpdateDisplay(string zenscapeUserJson)
    {
        try
        {
            JObject userObject = JObject.Parse(zenscapeUserJson);
            if (userObject != null)
            {
                // Retrieve and display baseline heart rate
                int baselineHeartRate = userObject["baseline_heart_rate"] != null ? userObject["baseline_heart_rate"].Value<int>() : 0;
                displayText.text += "Baseline Heart Rate: " + baselineHeartRate + "\n";
                // Retrieve and display live pulse
                int livePulse = userObject["live_pulse"] != null ? userObject["live_pulse"].Value<int>() : 0;
                displayText.text += "Live Pulse: " + livePulse + "\n";
                // Retrieve and display metrics of day 1, 2, and 3
                JArray metrics = userObject["metrics"] as JArray;

                UnityEngine.Debug.Log(metrics);

                if (metrics != null)
                {
                    int numberOfDays = metrics.Count; // Get the number of days' metrics available
                    int maxDaysToDisplay = Mathf.Min(numberOfDays, 3); // Determine the maximum number of days to display
                    for (int i = 1; i <= maxDaysToDisplay; i++)
                    {
                        JToken dayMetrics = metrics[i];
                        if (dayMetrics != null)
                        {
                            float avgPulse = dayMetrics["avg_pulse"] != null ? dayMetrics["avg_pulse"].Value<float>() : 0f;
                            displayText.text += "Day " + i + " Avg Pulse: " + avgPulse + "\n";

                            float coffee = dayMetrics["coffee"] != null ? dayMetrics["coffee"].Value<float>() : 0f;
                            displayText.text += "Day " + i + " Coffee Task Accuracy: " + coffee.ToString("P1") + "\n";

                            float phone = dayMetrics["phone"] != null ? dayMetrics["phone"].Value<float>() : 0f;
                            displayText.text += "Day " + i + " Phone Task Accuracy: " + phone.ToString("P1") + "\n";

                            float sort = dayMetrics["sort"] != null ? dayMetrics["sort"].Value<float>() : 0f;
                            displayText.text += "Day " + i + " Sorting Task Accuracy: " + sort.ToString("P1") + "\n";

                            float overall = dayMetrics["overall"] != null ? dayMetrics["overall"].Value<float>() : 0f;
                            displayText.text += "Day " + i + " Overall Accuracy: " + overall.ToString("P1") + "\n";
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Error parsing zenscape user data.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error updating display: " + e.Message);
        }
    }
}
