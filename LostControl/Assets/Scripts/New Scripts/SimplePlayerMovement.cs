using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 25f;  // Speed at which the player moves
    public float scaleSpeed = 0.1f; // Speed at which the player scales
    public float floatingForce = 5f; // Default force applied when the player is floating
    private HashSet<string> disabledAbilities = new HashSet<string>(); // Track disabled abilities
    private bool isFloating = false;  // Track if player is floating

    public Vector3 respawnPoint; // Point where the player respawns after dying

    private Rigidbody2D rb;          // Reference to the Rigidbody2D component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D for physics-based movement
        respawnPoint = transform.position;
    }

    void Update()
    {
        // Handle other movements
        HandleWalking();  // Handle walking input
        if (!disabledAbilities.Contains("Scale"))
        {
            HandleScaling(); // Handle scaling input
        }
    }

    void HandleWalking()
    {
        float move = Input.GetAxisRaw("Horizontal");
        if (!disabledAbilities.Contains("MoveLeft") && move < 0)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); // Move left
        }
        else if (!disabledAbilities.Contains("MoveRight") && move > 0)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y); // Move right
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Stop horizontal movement if unable
        }
    }

    void HandleScaling()
    {
        // Get the current scale of the player
        Vector3 currentScale = transform.localScale;

        // If the 'Q' key is pressed, scale up within the maximum limit
        if (Input.GetKey(KeyCode.Q))
        {
            currentScale += new Vector3(scaleSpeed, scaleSpeed, 0) * Time.deltaTime;
        }
        // If the 'S' key is pressed, scale down within the minimum limit
        else if (Input.GetKey(KeyCode.S))
        {
            currentScale -= new Vector3(scaleSpeed, scaleSpeed, 0) * Time.deltaTime;
        }

        // Clamp the scale values to the specified range
        currentScale.x = Mathf.Clamp(currentScale.x, 0.4f, 3f);
        currentScale.y = Mathf.Clamp(currentScale.y, 0.4f, 3f);

        // Apply the clamped scale to the transform
        transform.localScale = currentScale;
    }

    public void Fly()
    {
        isFloating = true;  // Set floating state to true
        float forceMultiplier = CalculateForceMultiplier(); // Calculate force based on player's size
        rb.velocity = new Vector2(rb.velocity.x, floatingForce * forceMultiplier); // Apply upward force with multiplier
    }

    public void Ground()
    {
        isFloating = false; // Set floating state to false
        rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity
    }

    // Calculate a multiplier for the jump force based on the player's scale
    private float CalculateForceMultiplier()
    {
        float currentScale = transform.localScale.x; // Assuming uniform scaling (x and y are equal)

        // Define the multiplier: smaller scale leads to lower jump force, larger scale to higher jump force
        if (currentScale >= 3f)
        {
            return 1.5f; // Larger player jumps higher
        }
        else if (currentScale <= 0.4f)
        {
            return 0.7f; // Smaller player jumps lower
        }
        else
        {
            // Calculate proportional multiplier between min and max scale range
            return Mathf.Lerp(0.7f, 1.5f, (currentScale - 0.4f) / (3f - 0.4f));
        }
    }

    public void SetAbility(string ability, bool enabled)
    {
        if (enabled)
        {
            disabledAbilities.Remove(ability); // Enable the ability
        }
        else
        {
            disabledAbilities.Add(ability); // Disable the ability
        }
    }

    public void EnableAllAbilities()
    {
        disabledAbilities.Clear(); // Clear the list to enable all abilities
    }
     public void Die()
    {
        // Example action: respawn the player at a specific point
        transform.position = respawnPoint;

        // Additional actions like reducing health or playing a death animation can be added here
        Debug.Log("Player has died and respawned!");
    }
}
