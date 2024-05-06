using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


[System.Serializable]
public class Task
{
    public GameObject gameObject;
    public string description;
    public bool completed;
}

public class ChecklistManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI checklistText; // Reference to the TextMeshProUGUI component
    [SerializeField] private List<Task> tasks1; // List of tasks to complete for the first set
    [SerializeField] private List<Task> tasks2; // List of tasks to complete for the second set
    private HashSet<GameObject> collectedObjects = new HashSet<GameObject>(); // Set of collected objects
    private bool secondSetUnlocked = false; // Flag to track if the second set is unlocked
    private int currentTaskIndex = 0; // Index of the current task the player should interact with
    private bool danceTriggered = false; // Flag to track if the dance has been triggered
    private float countdownTimer = 10f; // Timer for the countdown
    private bool levelCleared = false; // Flag to track if the level has been cleared

    public Animator snoopyAnimator; // Reference to the Snoopy's Animator component

    // Start is called before the first frame update
    void Start()
    {
        InitializeChecklistText(); // Initialize checklist text with default descriptions

        // Disable colliders of all tasks except the first one
        for (int i = 1; i < tasks1.Count; i++)
        {
            tasks1[i].gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelCleared && AllTasksCompleted(tasks1) && AllTasksCompleted(tasks2))
        {
            snoopyAnimator.SetTrigger("DanceTrigger");
            levelCleared = true;
            countdownTimer = 5f; // Set the countdown timer to 5 seconds
        }
        
        if (levelCleared)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0f)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                switch (currentSceneName)
                {
                    case "Level1":
                    SceneManager.LoadScene("NextLevel");
                    break;
                    
                    case "Level2":
                    SceneManager.LoadScene("complete");
                    break;
                    
                    default:
                    Debug.LogWarning("No scene defined for this level completion.");
                    break;
                }
            }
        }
    }



    // Method to initialize the checklist text with default descriptions for items
    private void InitializeChecklistText()
    {
        if (checklistText == null)
        {
            Debug.LogWarning("Checklist text is not set in the ChecklistManager script.");
            return;
        }

        string text = "Task List:\n";

        // Display the default descriptions for tasks1
        foreach (Task task in tasks1)
        {
            text += FormatTaskDescription(task) + "\n";
        }

        checklistText.text = text;
    }

    // Method to call when an object is picked up
    public void ObjectPickedUp(GameObject obj)
    {
        if (obj == null) // Check if the object is null
        {
            Debug.LogWarning("Object is null in ObjectPickedUp method.");
            return;
        }
        
        Task currentTask = GetCurrentTask();
        if (currentTask != null && currentTask.gameObject == obj) // Check if the object is the current task
        {
            collectedObjects.Add(obj); // Add the object to the collected set
            currentTask.completed = true; // Mark the current task as completed
            UpdateChecklistText(obj); // Update the checklist text for the picked up item
            
            // Determine which task list the current task belongs to
            List<Task> currentTaskList = GetCurrentTaskList();
            
            // Check if all objects from the current task list have been picked up
            if (AllTasksCompleted(currentTaskList))
            {
                // Unlock the next set of tasks if applicable
                if (currentTaskList == tasks1 && !secondSetUnlocked && AllTasksCompleted(tasks1))
                {
                    secondSetUnlocked = true; // Unlock the second set
                    ReplaceTasks1WithTasks2(); // Replace tasks from set 1 with tasks from set 2 in the checklist text
                }
            }
            // Enable the collider of the next task if it exists
            int nextTaskIndex = currentTaskList.IndexOf(currentTask) + 1;
            if (nextTaskIndex < currentTaskList.Count)
            {
                Collider collider = currentTaskList[nextTaskIndex].gameObject.GetComponent<Collider>();
                if (collider != null) // Check if the collider component exists
                {
                    collider.enabled = true; // Enable the collider
                }
                else
                {
                    Debug.LogWarning("Collider is null for the next task.");
                }
            }
            // Move to the next task
            currentTaskIndex++;
        }
        else
        {
        Debug.Log("Interacting with wrong task!"); // Log a message if interacting with the wrong task
        }
    }
    // Method to get the current task list
    private List<Task> GetCurrentTaskList()
    {
        if (currentTaskIndex < tasks1.Count)
        {
            return tasks1;
        }
        else if (secondSetUnlocked)
        {
            return tasks2;
        }
        else
        {
            return null;
        }
    }



    // Method to update the checklist text for the picked up item
    private void UpdateChecklistText(GameObject obj)
    {
        if (checklistText == null)
        {
            Debug.LogWarning("Checklist text is not set in the ChecklistManager script.");
            return;
        }

        string newText = "Task List:\n";

        // Determine which task list the current task belongs to
        List<Task> currentTaskList = GetCurrentTaskList();

        // Display the updated descriptions for the current task list
        foreach (Task task in currentTaskList)
        {
            newText += FormatTaskDescription(task) + "\n";
        }

        checklistText.text = newText; // Update the checklist text
    }

    // Method to format the task description with a tick mark if completed
    private string FormatTaskDescription(Task task)
    {
        if (task.completed)
        {
            return task.description + " - Done!";
        }
        else
        {
            return task.description;
        }
    }

    // Method to get the current task
    private Task GetCurrentTask()
    {
        if (currentTaskIndex < tasks1.Count)
        {
            return tasks1[currentTaskIndex];
        }
        else if (secondSetUnlocked && (currentTaskIndex - tasks1.Count) < tasks2.Count)
        {
            return tasks2[currentTaskIndex - tasks1.Count];
        }
        else
        {
            return null;
        }
    }

    // Method to check if all tasks are completed
    private bool AllTasksCompleted(List<Task> tasks)
    {
        foreach (Task task in tasks)
        {
            if (!task.completed)
            {
                return false;
            }
        }

        return true;
    }

    // Method to replace tasks from set 1 with tasks from set 2 in the checklist text
    private void ReplaceTasks1WithTasks2()
    {
        if (checklistText == null)
        {
            Debug.LogWarning("Checklist text is not set in the ChecklistManager script.");
            return;
        }

        string newText = "Task List:\n";

        // Display the items from the second set
        foreach (Task task in tasks2)
        {
            newText += FormatTaskDescription(task) + "\n";
        }

        checklistText.text = newText; // Update the checklist text with items from set 2
    }
    
}
