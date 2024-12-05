using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; // Speed of the platform
    public float distance = 3f; // Distance it moves from the starting position
    public float delay = 0f; // Delay before the platform starts moving
    public bool moveHorizontally = true; // Toggle for horizontal or vertical movement

    private Vector3 startPosition;
    private bool movingForward = true;
    private float delayTimer = 0f;

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
            return; // Skip movement until delay is complete
        }

        float movement = speed * Time.deltaTime;

        if (movingForward)
        {
            if (moveHorizontally)
                transform.position += new Vector3(movement, 0, 0); // Move horizontally
            else
                transform.position += new Vector3(0, movement, 0); // Move vertically

            if (Vector3.Distance(startPosition, transform.position) >= distance)
                movingForward = false;
        }
        else
        {
            if (moveHorizontally)
                transform.position -= new Vector3(movement, 0, 0); // Move horizontally
            else
                transform.position -= new Vector3(0, movement, 0); // Move vertically

            if (Vector3.Distance(startPosition, transform.position) <= 0.1f)
                movingForward = true;
        }
    }
}
