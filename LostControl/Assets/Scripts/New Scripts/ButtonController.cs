using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool isDragging = false;
    private Vector2 originalPosition; // Store the original canvas-relative position
    private RectTransform rectTransform; // RectTransform for UI positioning

    public SimplePlayerMovement playerMovement;
    public static bool isButtonOnScreen = false;
    public static ButtonController snappedButton = null;

    private bool isFloating = false;
    private float floatDuration = 1.5f;
    private float floatTimer = 1.5f;

    private bool canDragButton = true; // Flag to control whether the button can be dragged
    public static ButtonController currentlyDraggedButton = null; // Track the currently dragged button

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
        if (gameObject.name == "G Button" || (canDragButton && (!isButtonOnScreen || snappedButton == this)))
        {
            if (currentlyDraggedButton != null && currentlyDraggedButton != this)
                return;

            isDragging = true;
            currentlyDraggedButton = this; // Set this button as currently dragged
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
            currentlyDraggedButton = null; // Reset the currently dragged button
        }
    }

    void SnapToPrefabs(Vector2 mousePos)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("GridPrefab"))
        {
            if (!isButtonOnScreen || snappedButton == this)
            {
                isButtonOnScreen = true;
                snappedButton = this;

                transform.SetParent(worldSpaceCanvas, true);
                rectTransform.position = hit.collider.transform.position;

                CreatePlatformAtButtonPosition();

                if (gameObject.name == "A Button")
                {
                    playerMovement.SetCanMoveLeft(false); // Lock left movement
                }
                else if (gameObject.name == "D Button")
                {
                    playerMovement.SetCanMoveRight(false); // Lock right movement
                }
                else if (gameObject.name == "S Button")
                {
                    playerMovement.SetCanScale(false); // Disable scaling
                }
                else if (gameObject.name == "G Button")
                {
                    StartFloating(); // Trigger floating if G Button is snapped
                }
            }
        }
        else
        {
            ReturnToOriginalPosition();
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

        if (snappedButton == this)
        {
            isButtonOnScreen = false;
            snappedButton = null;
        }

        DeletePlatform();

        if (gameObject.name == "A Button")
        {
            playerMovement.SetCanMoveLeft(true); // Unlock left movement
        }
        else if (gameObject.name == "D Button")
        {
            playerMovement.SetCanMoveRight(true); // Unlock right movement
        }
        else if (gameObject.name == "S Button")
        {
            playerMovement.SetCanScale(true); // Enable scaling again
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
        if (!isButtonOnScreen)
        {
            playerMovement.SetCanMoveLeft(true);
            playerMovement.SetCanMoveRight(true);
            playerMovement.SetCanScale(true);
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

        // Detect mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 originalWorldPosition = originalParent.TransformPoint(originalPosition); // Convert to world position

            if (Vector2.Distance(mousePos, Camera.main.WorldToScreenPoint(originalWorldPosition)) < 10f)
            {
                ReturnToOriginalPosition();
            }
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
