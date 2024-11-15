using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ButtonController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool isDragging = false;
    private Vector2 originalPosition; // Store the original canvas-relative position
    private RectTransform rectTransform; // RectTransform for UI positioning

    public SimplePlayerMovement playerMovement;
    public static bool isButtonOnScreen = false;

    private bool isFloating = false;
    private float floatDuration = 1.5f;
    private float floatTimer = 1.5f;

    private bool canDragButton = true; // Flag to control whether the button can be dragged

    // Keep track of all snapped buttons
    public static List<ButtonController> snappedButtons = new List<ButtonController>();

    // Reference to the WorldSpaceCanvas
    public Transform worldSpaceCanvas;

    // Prefab for the GameObject to be created on snap
    public GameObject emptyObjectPrefab;

    // Reference to the platform created for this button
    private GameObject platformInstance;

    // Add this variable to store the original parent
    private Transform originalParent;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // Get RectTransform component
        originalPosition = rectTransform.anchoredPosition; // Save the initial canvas-relative position
        originalParent = transform.parent; // Store the original parent transform
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Check if the button can be dragged
        if (canDragButton && (snappedButtons.Contains(this) || gameObject.name != "G Button"))
        {
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform.parent, eventData.position, eventData.pressEventCamera, out mousePosition);
            rectTransform.anchoredPosition = mousePosition; // Set position in the canvas
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            SnapToPrefabs(eventData.position);
            isDragging = false;
        }
    }

    void SnapToPrefabs(Vector2 mousePos)
    {
        // Convert mouse position to world point
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0; // Set Z to zero for 2D

        // Perform a raycast to detect objects with the "SnapPoint" tag
        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, Vector2.zero);

        // Check if the raycast hits a "SnapPoint" object
        if (hit.collider != null && hit.collider.CompareTag("SnapPoint"))
        {
            Vector3 snapPointPosition = hit.collider.transform.position;
            Collider2D playerCollider = playerMovement.GetComponent<Collider2D>();

            if (!playerCollider.bounds.Intersects(hit.collider.bounds))
            {
                isButtonOnScreen = true;

                // Add this button to the list of snapped buttons
                snappedButtons.Add(this);

                transform.SetParent(worldSpaceCanvas, true);
                rectTransform.position = snapPointPosition;

                CreatePlatformAtButtonPosition();
                HandleButtonSpecificBehavior();
            }
            else
            {
                ReturnToOriginalPosition();
            }
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    private void HandleButtonSpecificBehavior()
    {
        if (gameObject.name == "A Button")
        {
            playerMovement.SetAbility("MoveLeft", false); // Lock left movement
        }
        else if (gameObject.name == "D Button")
        {
            playerMovement.SetAbility("MoveRight", false); // Lock right movement
        }
        else if (gameObject.name == "S Button")
        {
            playerMovement.SetAbility("Scale", false); // Disable scaling
        }
        else if (gameObject.name == "G Button")
        {
            StartFloating(); // Trigger floating if G Button is snapped
        }
    }

    private void CreatePlatformAtButtonPosition()
    {
        if (platformInstance != null)
        {
            Destroy(platformInstance);
        }

        platformInstance = Instantiate(emptyObjectPrefab, transform.position, Quaternion.identity, worldSpaceCanvas);

        BoxCollider2D boxCollider = platformInstance.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(1, 1); // Adjust size to fit your needs
        boxCollider.isTrigger = true; // Set to true if you want it to be a trigger
    }


    private void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;

        // Remove this button from the list of snapped buttons
        snappedButtons.Remove(this);

        if (snappedButtons.Count == 0)
        {
            isButtonOnScreen = false;
        }

        DeletePlatform();
        RestoreButtonSpecificBehavior();
    }

    private void RestoreButtonSpecificBehavior()
    {
        if (gameObject.name == "A Button")
        {
            playerMovement.SetAbility("MoveLeft", true); // Unlock left movement
        }
        else if (gameObject.name == "D Button")
        {
            playerMovement.SetAbility("MoveRight", true); // Unlock right movement
        }
        else if (gameObject.name == "S Button")
        {
            playerMovement.SetAbility("Scale", true); // Enable scaling again
        }
        else if (gameObject.name == "G Button")
        {
            playerMovement.Ground(); // Ensure the player is grounded after floating
        }

        canDragButton = true;
    }

    private void DeletePlatform()
    {
        if (platformInstance != null)
        {
            Destroy(platformInstance);
            platformInstance = null;
        }
    }

    public void ResetButton()
    {
        ReturnToOriginalPosition();
    }

    void Update()
    {
        // Check if no buttons are currently on screen
        if (snappedButtons.Count == 0)
        {
            playerMovement.EnableAllAbilities(); // Re-enable all abilities if no buttons are active
        }

        if (isFloating)
        {
            floatTimer += Time.deltaTime;
            if (floatTimer >= floatDuration)
            {
                isFloating = false;
                playerMovement.Ground();
                ResetButton();
            }
        }

        DetectMouseClick();
    }

    private void DetectMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 originalWorldPosition = Camera.main.WorldToScreenPoint(
                originalParent.TransformPoint(originalPosition)); // Convert to world position in screen space

            float distanceToOriginal = Vector2.Distance(mousePos, originalWorldPosition);

            if (distanceToOriginal < 50f) // Adjust the distance threshold based on sensitivity
            {
                ReturnToOriginalPosition();
            }
        }
    }

    public void OnButtonClick()
    {
        if (isButtonOnScreen && snappedButtons.Contains(this))
        {
            ReturnToOriginalPosition();
        }
    }

    private void StartFloating()
    {
        isFloating = true;
        floatTimer = floatDuration;
        playerMovement.Fly();  // Make the player float
        StartCoroutine(FloatingCoroutine());
    }

    private IEnumerator FloatingCoroutine()
    {
        while (floatTimer > 0)
        {
            floatTimer -= Time.deltaTime;
            yield return null;
        }
        isFloating = false;
        playerMovement.Ground();  // Return the player to the ground after floating
    }
}
