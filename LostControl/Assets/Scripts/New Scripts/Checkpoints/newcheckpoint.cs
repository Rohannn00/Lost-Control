using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newcheckpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the player's respawn point to this checkpoint
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.UpdateCheckpoint(transform.position);
            }
        }
    }
}
