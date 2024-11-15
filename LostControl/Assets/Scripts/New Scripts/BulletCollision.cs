using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the "Platform" tag
        if (collision.CompareTag("Clash"))
        {
            Debug.Log("Bullet collided with a platform!");
            // Handle bullet-platform collision logic here (e.g., destroy the bullet or the platform)
        }
    }
}
