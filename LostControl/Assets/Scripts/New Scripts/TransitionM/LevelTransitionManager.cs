using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTransitionManager : MonoBehaviour
{
    public string nextLevelName;// Name of the next level to load
    public float transitionTime = 1f; // Duration for the transition

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger zone
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone");
            // Start the level transition coroutine
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        Debug.Log("Loading next level: " + nextLevelName);
        // Optional: Wait for the duration of the transition
        yield return new WaitForSeconds(transitionTime);

        // Load the next level by name
        SceneManager.LoadScene(nextLevelName);
    }
}
