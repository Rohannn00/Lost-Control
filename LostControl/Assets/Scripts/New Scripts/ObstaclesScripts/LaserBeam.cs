using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public LineRenderer lineRenderer; // Reference to the Line Renderer
    public Transform laserOrigin; // Point from where the laser starts
    public float laserMaxDistance = 100f; // Maximum distance of the laser

    void Update()
    {
        // Set the start point of the laser to the laser origin
        lineRenderer.SetPosition(0, laserOrigin.position);

        // Cast a ray from the laser origin downwards
        RaycastHit2D hit = Physics2D.Raycast(laserOrigin.position, Vector2.down, laserMaxDistance);

        while (hit.collider != null && hit.collider.CompareTag ("SnapPoint"))
        {
            // If the ray hits "Grid Prefab", continue the raycast from the hit point
            hit = Physics2D.Raycast(hit.point + Vector2.down * 0.01f, Vector2.down, laserMaxDistance);
        }

        if (hit.collider != null)
        {
            // If the ray hits something other than "Grid Prefab", set the endpoint of the laser to the hit point
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
            // If the ray doesn't hit anything, set the endpoint to the max distance downwards
            lineRenderer.SetPosition(1, laserOrigin.position + Vector3.down * laserMaxDistance);
        }
    }
}
