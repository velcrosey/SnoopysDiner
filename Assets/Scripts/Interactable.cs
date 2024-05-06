using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string displayText = "Press 'E' to interact"; // Text to display
    [SerializeField] private float offsetY = 20f; // Offset from the bottom of the screen
    [SerializeField] private float textHeight = 30f; // Height of the text box
    [SerializeField] private GUISkin customSkin; // Custom GUI skin for styling
    [SerializeField] private AudioClip interactSound; // Sound effect for interaction
    [SerializeField] private ChecklistManager checklistManager; // Reference to the ChecklistManager script
    [SerializeField] private int taskIndex; // Index of the task in the ChecklistManager

    private bool playerInRange; // Flag to track if the player is in the trigger area
    private AudioSource audioSource; // Reference to the AudioSource component

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) // If player is in range and E key is pressed
        {
            Interact(); // Perform interaction
        }
    }

    // Method to perform interaction
    private void Interact()
    {
        Debug.Log("Interaction performed!");

        // Play the interaction sound effect
        if (audioSource != null && interactSound != null)
        {
            audioSource.PlayOneShot(interactSound);
        }

        // Mark the task as completed in the checklist manager
        checklistManager.ObjectPickedUp(gameObject);

    }

    // Called when another collider enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the entering collider is the player
        {
            playerInRange = true; // Set playerInRange flag to true
        }
    }

    // Called when another collider exits the trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the exiting collider is the player
        {
            playerInRange = false; // Set playerInRange flag to false
        }
    }

    // Draw GUI elements
    private void OnGUI()
    {
        if (!playerInRange) // If the player is not in range, do not display the text
            return;

        if (customSkin != null) // Apply custom GUI skin if available
        {
            GUI.skin = customSkin;
        }

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float boxWidth = screenWidth * 0.5f; // Width of the text box
        float boxX = (screenWidth - boxWidth) * 0.5f; // X position of the text box
        float boxY = screenHeight - offsetY - textHeight; // Y position of the text box

        // Draw the GUI box with the displayText at the specified position and size
        GUI.Box(new Rect(boxX, boxY, boxWidth, textHeight), displayText);
    }
}
