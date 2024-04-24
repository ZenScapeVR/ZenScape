using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class UserInitializer : MonoBehaviour
{
    public string firebaseURL = "https://zenscape-b6d91-default-rtdb.firebaseio.com/"; // Your Firebase URL
    public string activeFirebaseRef = "active_user"; // Firebase reference for "active"

    void Start()
    {
        // Call a coroutine to set active_user to a default value
        StartCoroutine(SetActiveUserDefault());
    }

    IEnumerator SetActiveUserDefault()
    {
        // Construct the URL for Firebase REST API
        string url = firebaseURL + "/" + activeFirebaseRef + ".json";

        // Construct the JSON data with default value (0 in this case)
        string jsonData = "{\"userId\": 1}";

        byte[] jsonDataBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = UnityWebRequest.Put(url, jsonDataBytes))
        {
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error setting active_user to default value: " + www.error);
            }
            else
            {
                Debug.Log("Set active_user to default value.");
            }
        }
    }
}
