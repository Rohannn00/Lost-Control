using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GButtonController : MonoBehaviour
{
    public SimplePlayerMovement playerMovement;
    public Button gButton; // Reference to the G button
    public float flyDuration = 3.0f; // How long the player flies before falling
    public float cooldownDuration = 5.0f; // How long the button is frozen after a click
    public Color fadeColor = new Color(1, 1, 1, 0.5f); // Faded color for cooldown

    private Color originalColor; // Store the original button color
    private bool isFlying = false; // To track if the player is currently flying
    private bool isOnCooldown = false; // To track if the button is on cooldown

    void Start()
    {
        // Ensure the Button component is attached and register the click event
        gButton = GetComponent<Button>();
        gButton.onClick.AddListener(OnGButtonClick);

        // Store the original button color
        originalColor = gButton.image.color;
    }

    void OnGButtonClick()
    {
        if (!isFlying && !isOnCooldown) // Check if not flying and not on cooldown
        {
            isFlying = true; // Set the flying state
            playerMovement.Fly(); // Enable flying

            // Change the button to faded color for cooldown
            gButton.image.color = fadeColor;

            // Start the fly timer and cooldown timer
            StartCoroutine(FlyForDuration(flyDuration));
            StartCoroutine(ButtonCooldown(cooldownDuration));
        }
    }

    private IEnumerator FlyForDuration(float duration)
    {
        yield return new WaitForSeconds(duration); // Wait for the specified fly time
        playerMovement.Ground(); // Stop flying and return to grounded state
        isFlying = false; // Reset flying state
    }

    private IEnumerator ButtonCooldown(float duration)
    {
        isOnCooldown = true; // Mark the button as on cooldown
        yield return new WaitForSeconds(duration); // Wait for the specified cooldown time

        // Reset the button's color to its original state
        gButton.image.color = originalColor;
        isOnCooldown = false; // Mark cooldown as over
    }
}
