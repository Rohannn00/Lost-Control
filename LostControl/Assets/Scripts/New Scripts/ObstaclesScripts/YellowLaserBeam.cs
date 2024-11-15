using UnityEngine;

public class YellowLaserBeam : MonoBehaviour
{
    public LineRenderer lineRenderer; // Reference to the Line Renderer
    public Transform laserOrigin; // Point from where the laser starts
    public float laserMaxDistance = 100f; // Maximum distance of the laser
    public float damageCooldown = 1.0f; // Cooldown time in seconds between damage

    private float lastDamageTime = -Mathf.Infinity; // Track when the player was last damaged

    void Update()
    {
        // Set the starting position of the laser
        lineRenderer.SetPosition(0, laserOrigin.position);

        // Shoot the laser downwards
        Vector2 rayDirection = Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(laserOrigin.position, rayDirection, laserMaxDistance);

        // Ignore SnapPoint collisions and continue raycasting
        while (hit.collider != null && hit.collider.CompareTag("SnapPoint"))
        {
            hit = Physics2D.Raycast(hit.point + rayDirection * 0.01f, rayDirection, laserMaxDistance);
        }

        // Check if the ray hits an object
        if (hit.collider != null)
        {
            // Set the ending position of the laser
            lineRenderer.SetPosition(1, hit.point);

            // Check if the laser hits the player
            if (hit.collider.CompareTag("Player"))
            {
                SimplePlayerMovement player = hit.collider.GetComponent<SimplePlayerMovement>();
                if (player != null && Time.time > lastDamageTime + damageCooldown)
                {
                    player.ReduceHealth(50); // Reduce player's health by 50
                    lastDamageTime = Time.time; // Update the last damage time
                }
            }
        }
        else
        {
            // If no collision, extend the laser to the maximum distance
            lineRenderer.SetPosition(1, laserOrigin.position + (Vector3)rayDirection * laserMaxDistance);
        }
    }
}
