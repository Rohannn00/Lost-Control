using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Call the Die() method on the player's script
            other.GetComponent<SimplePlayerMovement>().Die();
            Debug.Log("Player has touched the obstacle and died.");
        }
    }
}
