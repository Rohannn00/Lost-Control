using UnityEngine;

public class YellowLaserBeam : MonoBehaviour
{
    public LineRenderer lineRenderer; // Reference to the Line Renderer
    public Transform laserOrigin; // Point from where the laser starts
    public float laserMaxDistance = 100f; // Maximum distance of the laser
    public float damageCooldown = 1.0f; // Cooldown time in seconds between damage

    private float lastDamageTime = -Mathf.Infinity; // Track when the player was last damaged

    public enum LaserDirection { Up, Down, Left, Right } // Enum for laser direction
    public LaserDirection laserDirection = LaserDirection.Up; // Default direction

    void Update()
    {
        // Set the starting position of the laser
        lineRenderer.SetPosition(0, laserOrigin.position);

        // Determine the ray direction based on the selected laser direction
        Vector2 rayDirection = Vector2.zero;
        switch (laserDirection)
        {
            case LaserDirection.Up:
                rayDirection = Vector2.up;
                break;
            case LaserDirection.Down:
                rayDirection = Vector2.down;
                break;
            case LaserDirection.Left:
                rayDirection = Vector2.left;
                break;
            case LaserDirection.Right:
                rayDirection = Vector2.right;
                break;
        }

        // Cast the ray in the chosen direction
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
