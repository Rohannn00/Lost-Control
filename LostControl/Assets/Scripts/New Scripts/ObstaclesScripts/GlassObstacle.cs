using UnityEngine;

public class GlassObstacle : MonoBehaviour
{
    public float requiredScale = 3f; // The required scale for the player to break the glass

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the scale of the player
            Vector3 playerScale = collision.gameObject.transform.localScale;

            // Check if the player's scale is at or above the required scale
            if (playerScale.x >= requiredScale && playerScale.y >= requiredScale)
            {
                // Break the glass (destroy the game object)
                Destroy(gameObject);
                Debug.Log("Glass obstacle broken!");
            }
            else
            {
                // Optionally, add feedback when the player isn't large enough
                Debug.Log("Player is not big enough to break the glass.");
            }
        }
    }
}
