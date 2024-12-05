using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingPlat : MonoBehaviour
{
    public float speed = 2f; // Speed of the platform
    public float distance = 3f; // Distance it moves from the starting position
    public float delay = 0f; // Initial delay before the platform starts moving
    public float pauseTime = 1f; // Time the platform stays at the end position
    public bool moveHorizontally = true; // Toggle for horizontal or vertical movement

    private Vector3 startPosition;
    private bool movingForward = true;
    private float delayTimer = 0f;
    private float pauseTimer = 0f;
    private bool isPaused = false;

    void Start()
    {
        startPosition = transform.position;
        delayTimer = delay; // Initialize the delay timer
    }

    void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return; // Skip movement until initial delay is complete
        }

        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
            {
                isPaused = false;
            }
            return; // Skip movement while paused
        }

        float movement = speed * Time.deltaTime;

        if (movingForward)
        {
            if (moveHorizontally)
                transform.position += new Vector3(movement, 0, 0); // Move horizontally
            else
                transform.position += new Vector3(0, movement, 0); // Move vertically

            if (Vector3.Distance(startPosition, transform.position) >= distance)
            {
                movingForward = false;
                isPaused = true;
                pauseTimer = pauseTime; // Set pause time when reaching the end position
            }
        }
        else
        {
            if (moveHorizontally)
                transform.position -= new Vector3(movement, 0, 0); // Move horizontally
            else
                transform.position -= new Vector3(0, movement, 0); // Move vertically

            if (Vector3.Distance(startPosition, transform.position) <= 0.1f)
            {
                movingForward = true;
                isPaused = true;
                pauseTimer = pauseTime; // Set pause time when returning to the start position
            }
        }
    }
}
