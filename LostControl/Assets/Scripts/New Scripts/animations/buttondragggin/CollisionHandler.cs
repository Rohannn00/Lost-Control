using UnityEngine;
using Cinemachine;

public class CollisionHandler : MonoBehaviour
{
    public Animator boxAnimator; // Reference to the Animator component
    public GameObject dimOverlay; // Reference to the DimOverlay panel (Black screen)
    public GameObject spriteObject; // Reference to the sprite object with the Animator
    public GameObject player; // Reference to the player GameObject
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private bool isAnimationPlaying = false; // Track if the animation is playing
    private bool hasPlayedAnimation = false; // Track if the animation has already been played

    private Rigidbody2D playerRb; // Reference to the player's Rigidbody2D
    private SimplePlayerMovement playerMovementScript; // Reference to the player's movement script

    void Start()
    {
        // Get the SpriteRenderer component of the spriteObject
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();

        // Get the player's Rigidbody2D and movement script
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            playerMovementScript = player.GetComponent<SimplePlayerMovement>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // If the player enters the trigger and the animation hasn't played yet
        if (collider.CompareTag("Player") && !hasPlayedAnimation)
        {
            Debug.Log("Player entered the trigger area!");

            // Ensure the GameObject with the sprite is active (not deactivated)
            spriteObject.SetActive(true); // Make sure the GameObject is active

            // Enable the sprite renderer (if it's disabled, we want to show it)
            spriteRenderer.enabled = true;

            // Show the black screen overlay
            dimOverlay.SetActive(true);

            // Freeze the player
            FreezePlayer();

            // Freeze the camera movement
            FreezeCamera();

            // Play the animation
            if (boxAnimator != null)
            {
                boxAnimator.Play("buttondrag"); // Replace with your animation name
                isAnimationPlaying = true; // Mark the animation as playing
                hasPlayedAnimation = true; // Prevent animation from playing again
            }
        }
    }

    private void FreezePlayer()
    {
        if (playerRb != null)
        {
            // Stop the player's movement
            playerRb.velocity = Vector2.zero;
            playerRb.constraints = RigidbodyConstraints2D.FreezeAll; // Freeze all physics-based movement
        }

        if (playerMovementScript != null)
        {
            // Temporarily disable the player's movement script
            playerMovementScript.enabled = false;
        }
    }

    private void ResumePlayer()
    {
        if (playerRb != null)
        {
            // Restore physics-based movement
            playerRb.constraints = RigidbodyConstraints2D.FreezeRotation; // Allow movement, but no rotation
        }

        if (playerMovementScript != null)
        {
            // Re-enable the player's movement script
            playerMovementScript.enabled = true;
        }
    }

    private void FreezeCamera()
    {
        // Disable the Cinemachine Virtual Camera to freeze the camera
        if (virtualCamera != null)
        {
            virtualCamera.enabled = false;
        }
    }

    private void ResumeCamera()
    {
        // Enable the Cinemachine Virtual Camera again
        if (virtualCamera != null)
        {
            virtualCamera.enabled = true;
        }
    }

    private void ResumeGame()
    {
        // Resume the player movement and camera
        ResumePlayer();
        ResumeCamera();

        // Hide the black overlay
        dimOverlay.SetActive(false);

        // Stop the animation by disabling the Animator
        boxAnimator.enabled = false; // Disable the Animator component to stop the animation
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        // Check for mouse click to stop the animation and resume the game
        if (isAnimationPlaying && Input.GetMouseButtonDown(0)) // Detect mouse click
        {
            ResumeGame(); // Stop animation and hide the overlay

            // Reset the flag after handling the click
            isAnimationPlaying = false;
        }
    }
}
