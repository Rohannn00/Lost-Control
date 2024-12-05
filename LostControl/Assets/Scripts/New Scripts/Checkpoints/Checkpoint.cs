using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Animator animator;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();

            animator = GetComponent<Animator>();
            animator.SetTrigger("checkpoints");
            if (respawn != null)
            {
                // Update the player's respawn point to this checkpoint
                respawn.UpdateCheckpoint(transform.position);
                Debug.Log("Checkpoint updated to: " + transform.position);
            }
        }
    }
}
