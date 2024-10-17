using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Vector3 offset;    // Offset to keep the camera at a distance from the player
    public float smoothSpeed = 0.125f;  // Speed of the camera's smooth movement

    void LateUpdate()
    {
        // Target position based on player’s position and the specified offset
        Vector3 targetPosition = player.position + offset;

        // Smoothly move the camera towards the target position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
