using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeGameParent : MonoBehaviour
{
    public void EndGame(float accuracy){
        UnityEngine.Debug.Log("PARENT ENDING COFFEE TASK!");
        // Find the TaskSelection game object
        TaskSelection taskSelection = GameObject.FindObjectOfType<TaskSelection>();
        // Check if TaskSelection was found
        if (taskSelection != null)
        {
            UnityEngine.Debug.Log("PHONE PARENT OBJ: " + gameObject.name);
            taskSelection.EndTask(gameObject, accuracy);
            Destroy(gameObject);
        }
        else
        {
            UnityEngine.Debug.LogError("TaskSelection object not found!");
        }
    }
}
