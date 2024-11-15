using UnityEngine;

public class CanvasFollower : MonoBehaviour
{
    public Transform player; // Assign the player object in the inspector

    private Vector3 offset; // Initial offset between canvas and player

    void Start()
    {
        // Set the initial offset to maintain the relative position
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Update the position of the canvas to follow the player
        transform.position = player.position + offset;
    }
}
