using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LivePulseListener : MonoBehaviour
{
    // URL of the API endpoint
    private string apiUrl = "https://zenscape-b6d91-default-rtdb.firebaseio.com/pulse.json";

    // Reference to the TextMeshPro component
    public TextMeshProUGUI textMeshPro;

    // Frequency of API requests (in seconds)
    public float requestInterval = 1f; // Update every 1 second

    // Start is called before the first frame update
    void Start()
    {
        // Start coroutine to continuously fetch data from API
        StartCoroutine(FetchDataRoutine());
    }

    // Coroutine to continuously fetch data from API
    IEnumerator FetchDataRoutine()
    {
        while (true)
        {
            // Create a UnityWebRequest object
            using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
            {
                // Send the request and wait for the response
                yield return webRequest.SendWebRequest();

                // Check for errors
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    // Extract the numeric value from the JSON response
                    int value = ExtractNumericValue(webRequest.downloadHandler.text);

                    // Set the text of the TextMeshPro component to the extracted numeric value
                    textMeshPro.text = value.ToString();

                    // Log the numeric value
                    Debug.Log("Value: " + value);
                }
            }

            // Wait for 1 second before fetching data again
            yield return new WaitForSeconds(requestInterval);
        }
    }

    // Method to parse JSON response and extract numeric value of "value" field
    int ExtractNumericValue(string jsonResponse)
    {
        // Deserialize the JSON response into a data structure
        PulseData pulseData = JsonUtility.FromJson<PulseData>(jsonResponse);

        // Extract the numeric value of "value" field
        return pulseData.value;
    }

    // Data structure representing the JSON response
    [System.Serializable]
    public class PulseData
    {
        public int value;
    }
}
