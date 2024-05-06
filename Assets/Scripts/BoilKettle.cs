using UnityEngine;

public class BoilKettle : MonoBehaviour
{
    [SerializeField] private string displayText = "Press 'E' to boil the kettle"; // Text to display
    [SerializeField] private float offsetY = 20f; // Offset from the bottom of the screen
    [SerializeField] private float textHeight = 30f; // Height of the text box
    [SerializeField] private GUISkin customSkin; // Custom GUI skin for styling
    [SerializeField] private AudioClip boilingSound; // Sound effect for boiling water
    [SerializeField] private ParticleSystem steamParticles; // Reference to the steam Particle System
    [SerializeField] private ChecklistManager checklistManager; // Reference to the ChecklistManager script

    private bool playerInRange; // Flag to track if the player is in the trigger area
    private bool isBoiling; // Flag to track if the kettle is boiling
    private float boilTime = 15f; // Time it takes to boil the kettle (adjusted to 15 seconds)
    private float boilTimer; // Timer for boiling process
    private AudioSource audioSource; // Reference to the AudioSource component

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        // Disable steam particles at the start
        steamParticles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isBoiling) // If player is in range, E key is pressed, and kettle is not boiling
        {
            StartBoiling(); // Start boiling the kettle
        }

        if (isBoiling)
        {
            // Update the boiling timer
            boilTimer -= Time.deltaTime;

            if (boilTimer <= 0f)
            {
                // Boiling process completed
                FinishBoiling();
            }
        }
    }

    // Method to start boiling the kettle
    private void StartBoiling()
    {
        isBoiling = true;
        boilTimer = boilTime;
        Debug.Log("Kettle boiling started!");

        // Play the boiling water sound effect
        if (audioSource != null && boilingSound != null)
        {
            audioSource.PlayOneShot(boilingSound);
        }

        // Enable steam particles
        steamParticles.Play();
    }

    // Method to finish boiling the kettle
    private void FinishBoiling()
    {
        isBoiling = false;
        Debug.Log("Kettle is boiled!");

        // Stop steam particles
        steamParticles.Stop();

        // Add the boiled kettle to the checklist
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
        if (!playerInRange || isBoiling) // If the player is not in range or the kettle is boiling, do not display the text
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
