using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlatformController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    public SimplePlayerMovement playerMovement;
   /* public GameObject wButtonPrefab; // Reference to W Button prefab
    public GameObject sButtonPrefab; // Reference to S Button prefab
    public GameObject gButtonPrefab; // Reference to G Button prefab*/

    // Dictionary to hold button names and their corresponding prefabs
    public Dictionary<string, GameObject> buttonPrefabs = new Dictionary<string, GameObject>();

    void Start()
    {
        originalPosition = transform.position;

      /*  // Assign button prefabs to the dictionary
        buttonPrefabs.Add("W Button", wButtonPrefab);
        buttonPrefabs.Add("S Button", sButtonPrefab);
        buttonPrefabs.Add("G Button", gButtonPrefab);.*/
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Started on Platform: " + gameObject.name);
        // Optionally disable player movement while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging Platform: " + gameObject.name);
        // Move the platform based on world position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Maintain Z-axis
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if it's over a grid prefab
        if (IsOverGridPrefab())
        {
            SnapToGridPrefab();
        }
        else if (IsOverUICanvas(eventData.position))
        {
            // Reset to UI and convert back to button
          //  ResetAbility();

            string cleanName = gameObject.name.Replace("(Clone)", "");
            if (buttonPrefabs.ContainsKey(cleanName))
            {
                Instantiate(buttonPrefabs[cleanName], originalPosition, Quaternion.identity, transform.parent);
            }

            Destroy(gameObject); // Destroy the platform after resetting
        }
        else
        {
            // If not on grid prefab or UI, snap back to original position in game world
            transform.position = originalPosition;
        }
    }

    // Raycast to check if the platform is dropped over a grid prefab
    private bool IsOverGridPrefab()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("GridPrefab"))
        {
            return true;
        }

        return false;
    }

    // Snap the platform to the grid prefab's position
    private void SnapToGridPrefab()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("GridPrefab"))
        {
            transform.position = hit.collider.transform.position; // Snap to prefab's position
        }
    }

    private bool IsOverUICanvas(Vector2 mousePos)
    {
        // Perform a raycast from the mouse position
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = mousePos
        };

        // List to hold the results of the raycast
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        // Check if any of the raycast results hit a UI element
        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.CompareTag("UI"))
            {
                return true; // Return true if we are over a UI element
            }
        }

        return false; // If no UI element was hit, return false
    }

  /*  private void ResetAbility()
    {
        if (gameObject.name == "W Button")
        {
            playerMovement.SetCanWalk(true);
        }
        else if (gameObject.name == "S Button")
        {
            playerMovement.SetCanScale(true);
        }
        else if (gameObject.name == "G Button")
        {
            playerMovement.Ground(); // Reset floating for G Button
        }
    }*/
}
