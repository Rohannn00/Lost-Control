using UnityEngine;

public class GameEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("Game Over! Closing game."); // Debug message for testing

        // Quit the application
        Application.Quit();
    }
}
