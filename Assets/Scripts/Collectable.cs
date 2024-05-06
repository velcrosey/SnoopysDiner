using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private string displayText = "Press 'E' to pick up"; // Text to display
    [SerializeField] private float offsetY = 20f; // Offset from the bottom of the screen
    [SerializeField] private float textHeight = 30f; // Height of the text box
    [SerializeField] private GUISkin customSkin; // Custom GUI skin for styling
    [SerializeField] private AudioClip pickUpSound; // Sound to play when item is picked up
    [SerializeField] private ChecklistManager checklistManager; // Reference to the ChecklistManager script

    private bool playerInRange; // Flag to track if the player is in the trigger area

    // Update is called once per frame
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) // If player is in range and E key is pressed
        {
            Debug.Log("E key pressed"); // Check if the "E" key press is detected
            PickUpItem(); // Pick up item
        }
    }
    // Pick up item
    private void PickUpItem()
    {
        // Play pick up sound
        if (pickUpSound != null)
        {
            AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
        }
        
        // Call ObjectPickedUp method of the ChecklistManager script and pass the GameObject associated with this script
        checklistManager.ObjectPickedUp(gameObject);
        
        // Destroy the item
        Destroy(gameObject);
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
