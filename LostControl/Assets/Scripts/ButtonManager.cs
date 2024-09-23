using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
     private float GridSize = 135f; // Size of grid cells
    private float offset = 5f; // Space between snapped buttons


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / CanvasScalerReference(); // Drag the button
        canvasGroup.alpha = 0.6f; // Change transparency while dragging
        canvasGroup.blocksRaycasts = false; // Disable raycasting
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // Reset transparency
        canvasGroup.blocksRaycasts = true; // Enable raycasting
        SnapToGrid(); // Snap to grid on release
    }

    private void SnapToGrid()
    {
        Vector2 currentPos = rectTransform.anchoredPosition;

        // Calculate snapped position
        float snappedX = (Mathf.Round(currentPos.x / GridSize) * GridSize - offset)  ;
        float snappedY = (Mathf.Round(currentPos.y / GridSize) * GridSize - offset) ;


        // Apply snapped position
        rectTransform.anchoredPosition = new Vector2(snappedX, snappedY);
    }

    private float CanvasScalerReference()
    {
        // Reference the Canvas Scaler for appropriate scaling (if needed)
        // Replace with your canvas scaler reference logic if applicable
        return 1f;
    }
}