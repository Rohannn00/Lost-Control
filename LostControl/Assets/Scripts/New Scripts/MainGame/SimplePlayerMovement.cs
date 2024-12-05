using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 25f;  // Speed at which the player moves
    public float scaleSpeed = 0.1f; // Speed at which the player scales
    public float floatingForce = 5f; // Default force applied when the player is floating
    public Transform groundCheck;   // Transform to check if the player is grounded
    public float groundCheckRadius = 0.2f; // Radius for ground detection
    public LayerMask groundLayer;   // Layer mask to define what counts as ground
    private bool isGrounded = false; // Whether the player is on the ground
    private HashSet<string> disabledAbilities = new HashSet<string>(); // Track disabled abilities
    private bool isFloating = false;  // Track if player is floating
    private Vector3 currentCheckpoint; // The player's current checkpoint position
    private int health = 100;

    private Rigidbody2D rb;          // Reference to the Rigidbody2D component
    private PlayerRespawn playerRespawn;
    public GameObject _skeltalMeshrefrence;
    private Animator animator;       // Reference to the Animator component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D for physics-based movement
        currentCheckpoint = transform.position;
        playerRespawn = GetComponent<PlayerRespawn>();
        animator = _skeltalMeshrefrence.GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);

        // Handle other movements
        HandleWalking();  // Handle walking input
        if (!disabledAbilities.Contains("Scale"))
        {
            HandleScaling(); // Handle scaling input
        }
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        // Update the current checkpoint position
        currentCheckpoint = newCheckpoint;
    }

    void HandleWalking()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (move < 0 && !disabledAbilities.Contains("MoveLeft"))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); // Move left

            if (!isGrounded)
            {
                animator.SetBool("AirRun", true); // Trigger running animation while in air
            }
            else
            {
                animator.SetBool("IsRunning", true); // Trigger running animation on the ground
            }

            // Flip sprite to face left
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else if (move > 0 && !disabledAbilities.Contains("MoveRight"))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y); // Move right

            if (!isGrounded)
            {
                animator.SetBool("AirRun", true); // Trigger running animation while in air
            }
            else
            {
                animator.SetBool("IsRunning", true); // Trigger running animation on the ground
            }

            // Flip sprite to face right
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Stop horizontal movement
            animator.SetBool("IsRunning", false);
            animator.SetBool("AirRun", false);
        }
    }

    void HandleScaling()
    {
        // Get the current scale
        Vector3 currentScale = transform.localScale;

        // Preserve the X-axis sign for the direction (1 for right, -1 for left)
        float directionSign = Mathf.Sign(currentScale.x);

        // Temporarily use absolute values for uniform scaling
        float absScaleX = Mathf.Abs(currentScale.x);
        float absScaleY = currentScale.y;

        // Scale up when 'Q' is pressed
        if (Input.GetKey(KeyCode.Q))
        {
            absScaleX += scaleSpeed * Time.deltaTime;
            absScaleY += scaleSpeed * Time.deltaTime;
        }
        // Scale down when 'S' is pressed
        else if (Input.GetKey(KeyCode.S))
        {
            absScaleX -= scaleSpeed * Time.deltaTime;
            absScaleY -= scaleSpeed * Time.deltaTime;
        }

        // Clamp the scale values within the valid range
        absScaleX = Mathf.Clamp(absScaleX, 0.4f, 3f);
        absScaleY = Mathf.Clamp(absScaleY, 0.4f, 3f);

        // Apply the updated scale, restoring the direction for the X-axis
        currentScale.x = absScaleX * directionSign; // Reapply the sign for flipping
        currentScale.y = absScaleY;

        // Update the transform scale
        transform.localScale = currentScale;
    }

    public void Fly()
    {
        animator.SetBool("IsJumping", true);
        isFloating = true;  // Set floating state to true
        float forceMultiplier = CalculateForceMultiplier(); // Calculate force based on player's size
        rb.velocity = new Vector2(rb.velocity.x, floatingForce * forceMultiplier); // Apply upward force with multiplier
    }

    public void Ground()
    {
        animator.SetBool("IsJumping", false);
        isFloating = false; // Set floating state to false
        rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity
    }

    private float CalculateForceMultiplier()
    {
        float currentScale = transform.localScale.x;

        if (currentScale >= 3f) return 1.5f;
        if (currentScale <= 0.4f) return 0.7f;

        return Mathf.Lerp(0.7f, 1.5f, (currentScale - 0.4f) / (3f - 0.4f));
    }

    public void SetAbility(string ability, bool enabled)
    {
        if (enabled) disabledAbilities.Remove(ability);
        else disabledAbilities.Add(ability);
    }

    public void EnableAllAbilities()
    {
        disabledAbilities.Clear();
    }

    public void Die()
    {
        Debug.Log("Player has died!");
        health = 100;
        if (playerRespawn != null) playerRespawn.Respawn();
    }

    public void ReduceHealth(int amount)
    {
        health -= amount;
        if (health <= 0) Die();
        else Debug.Log("Player health: " + health);
    }
}
