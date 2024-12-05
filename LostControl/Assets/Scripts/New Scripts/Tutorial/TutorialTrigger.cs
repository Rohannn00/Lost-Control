using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject tutorialImage; // Assign the specific UI Image
    private bool hasTriggered = false;
    public float displayTime = 3f; // Time in seconds to show the image

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            tutorialImage.SetActive(true);
            Invoke("HideImage", displayTime); // Hide the image after a delay
        }
    }

    private void HideImage()
    {
        tutorialImage.SetActive(false);
    }
}
