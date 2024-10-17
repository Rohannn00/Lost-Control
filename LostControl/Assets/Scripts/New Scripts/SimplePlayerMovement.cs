using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 25f;  // Speed at which the player moves
    public float scaleSpeed = 0.1f; // Speed at which the player scales
    public float floatingForce = 5f; // Force applied when the player is floating
    private bool canMoveLeft = true;  // Flag to control left movement
    private bool canMoveRight = true; // Flag to control right movement
    private bool canScale = true;    // Flag to control scaling ability
    private bool isFloating = false;  // Track if player is floating

    private Rigidbody2D rb;          // Reference to the Rigidbody2D component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D for physics-based movement
    }

    void Update()
    {
        // Check if no buttons are snapped on the screen
        if (!ButtonController.isButtonOnScreen)
        {
            EnableAllAbilities(); // Re-enable all abilities if no buttons are active
        }

        HandleWalking();  // Handle walking input
        if (canScale)
        {
            HandleScaling(); // Handle scaling input
        }
    }

    void HandleWalking()
    {
        float move = Input.GetAxisRaw("Horizontal"); // Get horizontal input

        // Allow movement only based on the canMoveLeft and canMoveRight flags
        if ((move < 0 && canMoveLeft) || (move > 0 && canMoveRight))
        {
            // Set the horizontal velocity directly on the Rigidbody
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Stop horizontal movement if either direction is disabled
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void HandleScaling()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.localScale += new Vector3(scaleSpeed, scaleSpeed, 0) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, 0) * Time.deltaTime;
        }
    }

    // Methods to lock/unlock movement in specific directions
    public void SetCanMoveLeft(bool isEnabled)
    {
        canMoveLeft = isEnabled;
    }

    public void SetCanMoveRight(bool isEnabled)
    {
        canMoveRight = isEnabled;
    }

    public void SetCanScale(bool isEnabled)
    {
        canScale = isEnabled;
    }

    // Floating mechanics remain the same
    public void Fly()
    {
        isFloating = true;  // Set floating flag
        rb.gravityScale = 0;  // Disable gravity so the player doesn't fall
    }

    public void Ground()
    {
        isFloating = false;  // Stop floating
        rb.gravityScale = 1;  // Re-enable gravity
        rb.velocity = new Vector2(rb.velocity.x, 0);  // Ground the player by stopping vertical velocity
    }

    void FixedUpdate()
    {
        if (isFloating)
        {
            rb.velocity = new Vector2(rb.velocity.x, floatingForce);
        }
    }

    // Method to enable all player abilities
    private void EnableAllAbilities()
    {
        SetCanMoveLeft(true);
        SetCanMoveRight(true);
        SetCanScale(true);
    }
}
