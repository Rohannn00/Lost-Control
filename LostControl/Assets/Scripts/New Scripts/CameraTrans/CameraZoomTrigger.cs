using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraZoomTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera
    public float zoomedOutSize = 10f; // The orthographic size when zoomed out
    public float normalSize = 5f; // The normal orthographic size
    public float transitionSpeed = 2f; // Speed of the zoom transition

    public bool isZoomOutTrigger = true; // Set to true for zoom-out triggers and false for zoom-in triggers

    public Animator animator; // Reference to the Animator component
    public string animationTriggerName; // The trigger name for the animation
    public float animationDelay = 0.5f; // Delay in seconds before triggering the animation
    public float returnToNormalDelay = 1f; // Delay before returning the camera to its normal size

    private bool hasTriggered = false; // Flag to ensure this happens only once

    public GameObject player; // Reference to the player GameObject
    public MonoBehaviour playerMovementScript; // Reference to the player's movement script (if applicable)

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player")) // Check if it's the player and hasn't triggered yet
        {
            hasTriggered = true; // Set the flag to prevent retriggering
            StartCoroutine(HandleZoomOutThenReturn());
        }
    }

    private IEnumerator HandleZoomOutThenReturn()
    {
        // Freeze the player
        FreezePlayer(true);

        // Step 1: Zoom out the camera
        yield return StartCoroutine(ZoomCamera(zoomedOutSize));

        // Step 2: Trigger the animation after a delay
        yield return new WaitForSeconds(animationDelay);

        if (animator != null && !string.IsNullOrEmpty(animationTriggerName))
        {
            animator.SetTrigger(animationTriggerName);
        }

        // Step 3: Wait for the animation to complete (or delay)
        yield return new WaitForSeconds(returnToNormalDelay);

        // Step 4: Zoom the camera back to its original size
        yield return StartCoroutine(ZoomCamera(normalSize));

        // Unfreeze the player
        FreezePlayer(false);
    }

    private IEnumerator ZoomCamera(float targetSize)
    {
        float startSize = virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - targetSize) > 0.01f)
        {
            elapsedTime += Time.deltaTime;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime * transitionSpeed);
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetSize; // Ensure exact size at the end
    }

    private void FreezePlayer(bool freeze)
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = !freeze; // Disable or enable the movement script
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (freeze)
            {
                rb.velocity = Vector2.zero; // Stop the player's movement
                rb.constraints = RigidbodyConstraints2D.FreezeAll; // Freeze the Rigidbody
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Allow normal movement, keep rotation constraint
            }
        }
    }
}
