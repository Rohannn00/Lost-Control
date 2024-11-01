using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the player's current checkpoint to this position
            other.GetComponent<SimplePlayerMovement>().SetCheckpoint(transform.position);
            Debug.Log("Checkpoint reached: " + gameObject.name);
        }
    }
}
