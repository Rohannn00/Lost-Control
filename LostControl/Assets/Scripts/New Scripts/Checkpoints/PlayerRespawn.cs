using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRespawn : MonoBehaviour
{
    private Vector3 respawnPoint;

    private void Start()
    {
        // Set the initial respawn point to the player's starting position
        respawnPoint = transform.position;
    }

    // Update the checkpoint position
    public void UpdateCheckpoint(Vector3 newCheckpoint)
    {
        respawnPoint = newCheckpoint;
    }

    // Call this function when the player dies
    public void Respawn()
    {
        transform.position = respawnPoint;
    }
}


