using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public PlayerController playerController; // Reference to PlayerController
    public Canvas canvas; // Reference to the Canvas

    public RectTransform wButton, eButton, rButton;  // Reference to buttons
    private RectTransform draggedButton = null;  // Track the currently dragged button
    private bool isDragging = false;

    // Original positions for buttons
    private Vector3 wButtonOriginalPos, eButtonOriginalPos, rButtonOriginalPos;

    public float gridSize = 135f;  // Size of the grid (can be adjusted)
    private void Start()
    {
        // Store the original positions of the buttons
        wButtonOriginalPos = wButton.anchoredPosition;
        eButtonOriginalPos = eButton.anchoredPosition;
        rButtonOriginalPos = rButton.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Only allow dragging if no other button is being dragged
        if (isDragging || draggedButton != null)
            return;

        // Set the current dragged button based on the event
        if (eventData.pointerDrag == wButton.gameObject)
        {
            draggedButton = wButton;
            
        }
        else if (eventData.pointerDrag == eButton.gameObject)
        {
            draggedButton = eButton;
           
        }
        else if (eventData.pointerDrag == rButton.gameObject)
        {
            draggedButton = rButton;
            
        }

        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedButton == null) return;

        // Drag the button within the game frame
        Vector3 newPos = draggedButton.anchoredPosition + (Vector2)eventData.delta / canvas.scaleFactor;

        // Get the canvas size (entire screen size)
        float canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;

        // Define game frame boundaries
        float gameFrameHeight = 972f;  // Upper limit for dragging (90% of the screen height)
        float bottomLimit = 270f;      // Lower limit (button area)
        float leftLimit = 0f;          // Left boundary
        float rightLimit = canvasWidth - draggedButton.rect.width;  // Right boundary

        // Clamp the button position within the game frame
        newPos.x = Mathf.Clamp(newPos.x, leftLimit, rightLimit);
        newPos.y = Mathf.Clamp(newPos.y, bottomLimit, gameFrameHeight);

        draggedButton.anchoredPosition = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedButton == null) return;

        // Snap the button to the nearest grid position
        SnapToGrid(draggedButton);

        isDragging = false;
    }
    private void SnapToGrid(RectTransform button)
    {
        Vector3 snappedPosition = button.anchoredPosition;

        // Calculate the snapped position based on the grid size
        snappedPosition.x = Mathf.Round(snappedPosition.x / gridSize) * gridSize;
        snappedPosition.y = Mathf.Round(snappedPosition.y / gridSize) * gridSize;

        // Assign the snapped position to the button
        button.anchoredPosition = snappedPosition;
    }
    // Method to ensure the button stays within the lower screen bounds
    private Vector2 ClampToLowerScreen(Vector2 position)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Calculate the lower half of the screen as the bounds
        float minX = 0;
        float maxX = canvasRect.rect.width;
        float minY = 0;
        float maxY = canvasRect.rect.height / 2; // Limit to lower half of the screen

        // Clamp the button's position to stay within the lower half of the screen
        return new Vector2(
            Mathf.Clamp(position.x, minX, maxX),
            Mathf.Clamp(position.y, minY, maxY)
        );
    }

    // Method to snap the button to the grid
    private void SnapButtonToGrid(RectTransform buttonTransform)
    {
        float gridSize = 135f; // Adjust as needed
        Vector2 snappedPosition = new Vector2(
            Mathf.Round(buttonTransform.anchoredPosition.x / gridSize) * gridSize,
            Mathf.Round(buttonTransform.anchoredPosition.y / gridSize) * gridSize
        );

        buttonTransform.anchoredPosition = snappedPosition;
    }
        public void ReturnButtonToOriginalPosition()
        {
            // Reset the button to its original position
            if (draggedButton == wButton)
            {
                wButton.anchoredPosition = wButtonOriginalPos;
            }
            else if (draggedButton == eButton)
            {
                eButton.anchoredPosition = eButtonOriginalPos;
            }
            else if (draggedButton == rButton)
            {
                rButton.anchoredPosition = rButtonOriginalPos;
            }

            // Allow other buttons to be dragged again
            draggedButton = null;
            isDragging = false;
        }
    }
    
