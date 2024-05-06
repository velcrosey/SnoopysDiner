using UnityEngine;

public class Oven : MonoBehaviour
{
    [SerializeField] private string displayText = "Press 'E' to use the oven"; // Text to display
    [SerializeField] private float offsetY = 20f; // Offset from the bottom of the screen
    [SerializeField] private float textHeight = 30f; // Height of the text box
    [SerializeField] private GUISkin customSkin; // Custom GUI skin for styling
    [SerializeField] private AudioClip cookingSound; // Sound effect for cooking
    [SerializeField] private ParticleSystem cookingParticles; // Reference to the cooking Particle System
    [SerializeField] private ChecklistManager checklistManager; // Reference to the ChecklistManager script

    private bool playerInRange; // Flag to track if the player is in the trigger area
    private bool isCooking; // Flag to track if the oven is cooking
    private float cookTime = 2f; // Time it takes to cook (adjusted to 20 seconds)
    private float cookTimer; // Timer for cooking process
    private AudioSource audioSource; // Reference to the AudioSource component

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        // Disable cooking particles at the start
        cookingParticles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isCooking) // If player is in range, E key is pressed, and oven is not cooking
        {
            StartCooking(); // Start cooking with the oven
        }

        if (isCooking)
        {
            // Update the cooking timer
            cookTimer -= Time.deltaTime;

            if (cookTimer <= 0f)
            {
                // Cooking process completed
                FinishCooking();
            }
        }
    }

    // Method to start cooking with the oven
    private void StartCooking()
    {
        isCooking = true;
        cookTimer = cookTime;
        Debug.Log("Oven cooking started!");

        // Play the cooking sound effect
        if (audioSource != null && cookingSound != null)
        {
            audioSource.PlayOneShot(cookingSound);
        }

        // Enable cooking particles
        cookingParticles.Play();
    }

    // Method to finish cooking with the oven
    private void FinishCooking()
    {
        isCooking = false;
        Debug.Log("Oven cooking finished!");

        // Stop cooking particles
        cookingParticles.Stop();

        // Add the oven to the checklist
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
        if (!playerInRange || isCooking) // If the player is not in range or the oven is cooking, do not display the text
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
