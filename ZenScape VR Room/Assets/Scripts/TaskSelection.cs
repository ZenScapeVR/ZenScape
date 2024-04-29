using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskSelection : MonoBehaviour
{
    public AudioClip newTaskAlert; // Audio clip to play when a new task is assigned
    public int Day = 1;
    public bool AllTasksComplete = false;
    public int timerLength;
    public ZenscapeTimer timer;
    public TextMeshProUGUI display;
    public int numberOfTasks = 3;
    private int tasksAssigned = 0;
    public GameObject[] Tasks; // all tasks available throughout entire game
    private GameObject[] liveTasks; // all tasks currently active for the user
    private GameObject[] availableTasks; // all tasks NOT TAKEN or ENDED

    void Start()
    {
        liveTasks = new GameObject[0]; 
        // Attach an AudioSource component if one doesn't exist
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
        AllTasksComplete = false;
        timer.TimeRemaining = timerLength; // 4 minutes
        availableTasks = Tasks;
        // Start coroutine to spawn tasks with random time delay
        StartCoroutine(SpawnTasksWithDelay());
    }

   IEnumerator SpawnTasksWithDelay()
{
    while (!AllTasksComplete)
    {
        // Generate a random time between tasks
        float timeBetweenTasks = Random.Range(10f, 15f);
        yield return new WaitForSeconds(timeBetweenTasks);
        UnityEngine.Debug.Log("Task Spawn waiting for " + timeBetweenTasks + " seconds...");
        // Spawn a task
        SpawnTask();
    }
}

void SpawnTask()
{
    if (tasksAssigned < numberOfTasks)
    {
        if (availableTasks.Length > 0)
        {
            // Randomly select a task from availableTasks array
            int randomIndex = Random.Range(0, availableTasks.Length);
            GameObject taskPrefab = availableTasks[randomIndex];

            UnityEngine.Debug.Log("Task Chosen! Task is: " + taskPrefab.name);
            // Instantiate the selected task
            GameObject newTask = Instantiate(taskPrefab, taskPrefab.transform.position, taskPrefab.transform.rotation);

            // Add the spawned task to liveTasks
            List<GameObject> tempLiveList = new List<GameObject>(liveTasks);
            tempLiveList.Add(taskPrefab);
            liveTasks = tempLiveList.ToArray();
            tasksAssigned++;

            // Remove the spawned task from availableTasks
            List<GameObject> tempAvailList = new List<GameObject>(availableTasks);
            tempAvailList.RemoveAt(randomIndex);
            availableTasks = tempAvailList.ToArray();
            UpdateDisplay();

            // Play newTaskAlert audio clip
            if (newTaskAlert != null)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.PlayOneShot(newTaskAlert);
                UnityEngine.Debug.Log("Played newTaskAlert audio clip.");
            }
        }
        else
        {
            StartCoroutine(WaitForTaskToBecomeAvailable());
        }
    }
    else
    {
        UnityEngine.Debug.Log("All tasks assigned.");
    }
}

    IEnumerator WaitForTaskToBecomeAvailable()
    {
        yield return new WaitUntil(() => availableTasks.Length > 0);
        SpawnTask();
    }


    public void UpdateDisplay(){

        if(AllTasksComplete){
            display.text =  "\tAll Tasks Complete! Great Job!"; 
        }else{
            display.text =  "\tTime Left: " +  timer.TimeRemaining
            +   "\n\tTasks To Do:\n\t";
            foreach( GameObject task in liveTasks){
                display.text += "- " + task.name + "\n\t";
            }
        }
    }

    public void EndTask(GameObject taskObject, float accuracy)
    {
        string tempTaskName = taskObject.name;
        // Check if the task name ends with "(Clone)"
        if (tempTaskName.EndsWith("(Clone)"))
        {
            // If it does, remove "(Clone)" from the end of the name
            UnityEngine.Debug.Log("Removing task clone substring");
            tempTaskName = tempTaskName.Substring(0, tempTaskName.Length - 7);
        }
        UnityEngine.Debug.Log("End task called with game: " + tempTaskName + " and " + accuracy);
        
        foreach(GameObject task in Tasks){
            if(tempTaskName == task.name){
                UnityEngine.Debug.Log("FOUND MATCHING TASK NAME!");
                // Remove the task from liveTasks
                List<GameObject> tempList = new List<GameObject>(liveTasks);
                tempList.Remove(task);
                liveTasks = tempList.ToArray();
                // Add the task back to availableTasks
                  UnityEngine.Debug.Log("Adding: " + task.name + " back to available tasks.");
                List<GameObject> tempList2 = new List<GameObject>(availableTasks);
                tempList2.Add(task);
                availableTasks = tempList2.ToArray();
                Destroy(taskObject); // rid of task.
                UpdateDisplay();
            }
        }
        // Check if all tasks are complete
        if (liveTasks.Length == 0 && tasksAssigned == numberOfTasks)
        {
            AllTasksComplete = true;
            EndDay();
            // Perform any actions needed when all tasks are complete
        }
    }


    public void EndDay(){
        UnityEngine.Debug.Log("All tasks completed, day over!");
        // when the day is over, we need to record the accuracy metrics into firebase.
    }
}
