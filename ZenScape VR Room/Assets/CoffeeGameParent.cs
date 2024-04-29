using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CoffeeGameParent : MonoBehaviour
{

    public TextMeshPro CoffeeMetrics;
    public void MetricsAndEndGame(int coffeesDrank){
        UnityEngine.Debug.Log("Updating Coffee Metrics From Parent!");
        float accuracy = (coffeesDrank / 1f) * 100;
        CoffeeMetrics.text = "Coffees Drank Correctly: " + coffeesDrank;
        EndGame(accuracy);
    }

    public void EndGame(float accuracy){
        UnityEngine.Debug.Log("PARENT ENDING COFFEE TASK!");
        // Find the TaskSelection game object
        TaskSelection taskSelection = GameObject.FindObjectOfType<TaskSelection>();
        // Check if TaskSelection was found
        if (taskSelection != null)
        {
            UnityEngine.Debug.Log("TASK SELECTION PARENT OBJ: " + gameObject.name);
            taskSelection.EndTask(gameObject, accuracy);
            Destroy(gameObject);
        }
        else
        {
            UnityEngine.Debug.LogError("TaskSelection object not found!");
        }
    }
}
