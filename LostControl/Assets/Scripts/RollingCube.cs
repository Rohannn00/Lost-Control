using System.Collections;
using UnityEngine;

public class ClimbingCube : MonoBehaviour
{
    public float rollSpeed = 5f;  // Speed of rolling
    public LayerMask groundLayer; // Layer to detect ground/platforms
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isClimbing = false;
    private float cubeSize;
    private bool isRolling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cubeSize = GetComponent<Collider2D>().bounds.size.x; // Assuming it's a square cube
    }

    void Update()
    {
        // Check if cube is grounded by casting multiple rays (center and edges)
        isGrounded = CheckGrounded();

        // Allow rolling or climbing only when grounded and not already in the middle of an action
        if (isGrounded && !isRolling && !isClimbing)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                StartCoroutine(Roll(Vector2.right));
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                StartCoroutine(Roll(Vector2.left));
            }
            else if (Input.GetKey(KeyCode.UpArrow))  // Detect when Up Arrow is pressed
            {
                // Detect block and initiate climbing
                RaycastHit2D wallHit = Physics2D.Raycast(transform.position, Vector2.right, cubeSize / 2 + 0.1f, groundLayer);
                if (wallHit.collider != null)  // If we detect a wall in front of the cube
                {
                    StartCoroutine(Climb(Vector2.up, wallHit.collider));
                }
            }
        }
    }


    // Coroutine to handle smooth rolling
    IEnumerator Roll(Vector2 direction)
    {
        isRolling = true;

        // Set up the rotation point (corner) for the cube
        Vector2 rotationPoint = direction == Vector2.right
            ? new Vector2(cubeSize / 2, -cubeSize / 2)
            : new Vector2(-cubeSize / 2, -cubeSize / 2);
        rotationPoint += (Vector2)transform.position;

        // Rotate by 90 degrees to simulate one "roll"
        float angle = 0f;
        while (angle < 90f)
        {
            float rotateAmount = rollSpeed * Time.deltaTime * 90f; // 90 degrees per roll
            transform.RotateAround(rotationPoint, Vector3.forward, -rotateAmount * direction.x);
            angle += rotateAmount;

            yield return null;  // Wait for the next frame
        }

        // Snap the rotation to a perfect 90 degrees
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Round(transform.rotation.eulerAngles.z / 90) * 90);

        // Snap position to the grid to prevent floating-point errors
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);

        isRolling = false;
    }

    // Coroutine to handle smooth climbing
    IEnumerator Climb(Vector2 direction, Collider2D wall)
    {
        isClimbing = true;

        // Set the rotation point (top corner of the block) for climbing
        Vector2 rotationPoint = new Vector2(transform.position.x + cubeSize / 2, wall.bounds.max.y);

        float angle = 0f;
        while (angle < 90f)
        {
            float rotateAmount = rollSpeed * Time.deltaTime * 90f;
            transform.RotateAround(rotationPoint, Vector3.forward, rotateAmount);
            angle += rotateAmount;

            yield return null;
        }

        // Snap position to be perfectly aligned to the top of the block after climbing
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector2(wall.bounds.max.x + cubeSize / 2, wall.bounds.max.y + cubeSize / 2);

        isClimbing = false;
    }

    // Improved Ground Detection using multiple Raycasts
    private bool CheckGrounded()
    {
        float rayLength = cubeSize / 2 + 0.1f; // Slightly longer than half the cube size
        Vector2 leftEdge = new Vector2(transform.position.x - cubeSize / 2, transform.position.y);
        Vector2 rightEdge = new Vector2(transform.position.x + cubeSize / 2, transform.position.y);
        Vector2 center = transform.position;

        RaycastHit2D leftRay = Physics2D.Raycast(leftEdge, Vector2.down, rayLength, groundLayer);
        RaycastHit2D rightRay = Physics2D.Raycast(rightEdge, Vector2.down, rayLength, groundLayer);
        RaycastHit2D centerRay = Physics2D.Raycast(center, Vector2.down, rayLength, groundLayer);

        Debug.DrawRay(leftEdge, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(rightEdge, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(center, Vector2.down * rayLength, Color.red);

        return leftRay.collider != null || rightRay.collider != null || centerRay.collider != null;
    }
}
