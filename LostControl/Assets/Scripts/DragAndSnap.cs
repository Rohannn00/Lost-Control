using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndSnap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum Ability { Walk, Rotate, Scale }

    private Vector2 originalPosition;
    private bool isSnapped = false;
    private bool isOutOfGameArea = false;
    private bool isDragging = false;

    public PlayerController playerController; // Reference to PlayerController script

    private RectTransform rectTransform; // For UI position snapping
    public Canvas canvas; // Reference to the canvas
    public float bottomBoundary = -3f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // Check if dragged out of game area
        if (isOutOfGameArea && !RectTransformUtility.RectangleContainsScreenPoint(canvas.GetComponent<RectTransform>(), Input.mousePosition))
        {
           // SnapBack();
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        isSnapped = false; // Reset snapped state
    }

    public void OnDrag(PointerEventData eventData)
    {
        
            if (!isDragging || isSnapped)
                return;

            // Get the mouse position and convert it to the canvas world position
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, canvas.worldCamera, out mousePos);

            // Move the button with the drag, but clamp to keep it within visible canvas area
            Vector2 clampedPosition = ClampToCanvas(mousePos);

            // Update the button's position
            rectTransform.anchoredPosition = clampedPosition;
     
     
    }
    private Vector2 ClampToCanvas(Vector2 mousePos)
    {
        // Get the dimensions of the canvas RectTransform
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Find the boundaries within which the button can move (considering the button's size)
        float halfWidth = rectTransform.rect.width / 2;
        float halfHeight = rectTransform.rect.height / 2;

        float clampedX = Mathf.Clamp(mousePos.x, -canvasRect.rect.width / 2 + halfWidth, canvasRect.rect.width / 2 - halfWidth);
       float clampedY = Mathf.Clamp(mousePos.y, -canvasRect.rect.height / 2 + halfHeight, canvasRect.rect.height / 2 - halfHeight * 0.25f);
        return new Vector2(clampedX, clampedY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // Check if dropped in a valid snap area
        if (InNoSnapArea())
        {
            //SnapBack(); // Return button if not valid
        }
        else
        {
            isSnapped = true;
             // Hide button once snapped
            LockAbility(); // Disable the corresponding ability
        }
    }

    private bool InNoSnapArea()
    {
       
        if (rectTransform.anchoredPosition.y < bottomBoundary)
        {
            return true; // The button is in the "no snap" area
        }
        return false; 
    }

    private void LockAbility()
    {
        // Lock specific player ability based on the button's name
        if (gameObject.name == "Button_W")
        {
            playerController.SetAbility(PlayerController.Ability.Walk, false);
        }
        else if (gameObject.name == "Button_E")
        {
            playerController.SetAbility(PlayerController.Ability.Rotate, false);
        }
        else if (gameObject.name == "Button_R")
        {
            playerController.SetAbility(PlayerController.Ability.Scale, false);
        }
    }

   /* private void SnapBack()
    {
        // Return button to original position and unlock the ability
        isSnapped = false;
        rectTransform.anchoredPosition = originalPosition;
        UnlockAbility();
        gameObject.SetActive(true); // Make button visible again
    }
   */
    private void UnlockAbility()
    {
        // Unlock specific player ability based on the button's name
        if (gameObject.name == "Button_W")
        {
            playerController.SetAbility(PlayerController.Ability.Walk, true);
        }
        else if (gameObject.name == "Button_E")
        {
            playerController.SetAbility(PlayerController.Ability.Rotate, true);
        }
        else if (gameObject.name == "Button_R")
        {
            playerController.SetAbility(PlayerController.Ability.Scale, true);
        }
    }
    private void OnDrawGizmos()
    {
        // Draw a red line at the bottom boundary to visualize it in the Scene view
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            Gizmos.color = Color.red;
            Vector3 leftPoint = new Vector3(canvasRect.rect.xMin, bottomBoundary, 0);
            Vector3 rightPoint = new Vector3(canvasRect.rect.xMax, bottomBoundary, 0);
            Gizmos.DrawLine(leftPoint, rightPoint);
        }
    }
}
