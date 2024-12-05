using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTransitionManager : MonoBehaviour
{
    public float transitionTime = 1.5f; // Time for the transition animation
    public Animator transition;        // Animator controlling the transition animations
    public string nextLevel;           // Name of the next level to load

    private static bool shouldPlayTransitionIn = true; // Track whether to play Transition IN at scene start

    private void Start()
    {
        // Play Transition IN animation if required
        if (shouldPlayTransitionIn && transition != null)
        {
            transition.SetTrigger("IN");
            shouldPlayTransitionIn = false; // Reset after playing
        }
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        // Set the flag to play Transition IN in the next scene
        shouldPlayTransitionIn = true;

        // Start the coroutine for the transition and scene loading
        StartCoroutine(LoadLevelCoroutine());
    }

    private IEnumerator LoadLevelCoroutine()
    {
        // Trigger Transition OUT animation
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        // Wait for the transition animation to complete
        yield return new WaitForSeconds(transitionTime);

        // Load the next scene
        if (!string.IsNullOrEmpty(nextLevel))
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            Debug.LogError("Next level name is not set! Please assign it in the inspector.");
        }
    }
}
