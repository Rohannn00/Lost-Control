using UnityEngine;
using UnityEngine.UI;

public class CustomSnapping : MonoBehaviour
{
    public float gridSize = 10f;
    public float snapThreshold = 0.1f;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Calculate the snapped position
        Vector2 snappedPosition = new Vector2(
            Mathf.Round(rectTransform.anchoredPosition.x / gridSize) * gridSize,
            Mathf.Round(rectTransform.anchoredPosition.y / gridSize) * gridSize
        );

        // Check if the snapped position is within the snap threshold
        if (Vector2.Distance(rectTransform.anchoredPosition, snappedPosition) < snapThreshold)
        {
            // Snap the button to the snapped position
            rectTransform.anchoredPosition = snappedPosition;
        }
    }
}