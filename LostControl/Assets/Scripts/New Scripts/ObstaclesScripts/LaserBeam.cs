using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public LineRenderer lineRenderer; // Reference to the Line Renderer
    public Transform laserOrigin; // Point from where the laser starts
    public float laserMaxDistance = 100f; // Maximum distance of the laser

    [Header("Direction Settings")]
    public bool shootHorizontally = false; // Toggle for horizontal or vertical raycasting

    void Update()
    {
        // Set the start point of the laser to the laser origin
        lineRenderer.SetPosition(0, laserOrigin.position);

        // Determine the ray direction based on the chosen setting
        Vector2 rayDirection = shootHorizontally ? Vector2.right : Vector2.down;

        // Cast a ray from the laser origin in the specified direction
        RaycastHit2D hit = Physics2D.Raycast(laserOrigin.position, rayDirection, laserMaxDistance);

        // Check for consecutive hits on objects tagged "SnapPoint"
        while (hit.collider != null && hit.collider.CompareTag("SnapPoint"))
        {
            // Continue the raycast from the hit point
            hit = Physics2D.Raycast(hit.point + rayDirection * 0.01f, rayDirection, laserMaxDistance);
        }

        if (hit.collider != null)
        {
            // If the ray hits something other than "SnapPoint," set the endpoint of the laser to the hit point
            lineRenderer.SetPosition(1, hit.point);

            // Check if the hit object is a player and handle the collision
            if (hit.collider.CompareTag("Player"))
            {
                // Handle player death or damage
                hit.collider.GetComponent<SimplePlayerMovement>().Die();
            }
        }
        else
        {
            // If the ray doesn't hit anything, set the endpoint to the max distance in the chosen direction
            lineRenderer.SetPosition(1, laserOrigin.position + (Vector3)rayDirection * laserMaxDistance);
        }
    }
}