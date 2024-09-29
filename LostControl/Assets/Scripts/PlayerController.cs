using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum Ability { Walk, Rotate, Scale, All }

    // UI Buttons
    public Button walkButton;
    public Button rotateButton;
    public Button scaleButton;

    // Store original button positions
    private Vector3 originalWalkPos;
    private Vector3 originalRotatePos;
    private Vector3 originalScalePos;

    private bool canWalk = true;
    private bool canRotate = true;
    private bool canScale = true;

    public float movespeed = 25f;
    public float rotatespeed = 25f;
    public float scalespeed = 25f;

    void Start()
    {
        // Store initial positions of buttons
        originalWalkPos = walkButton.transform.position;
        originalRotatePos = rotateButton.transform.position;
        originalScalePos = scaleButton.transform.position;

        // Set up button listeners (if needed for extra actions)
        walkButton.onClick.AddListener(() => SetAbility(Ability.Walk, false));   // Disable walking when button clicked
        rotateButton.onClick.AddListener(() => SetAbility(Ability.Rotate, false)); // Disable rotating when button clicked
        scaleButton.onClick.AddListener(() => SetAbility(Ability.Scale, false)); // Disable scaling when button clicked
    }

    void Update()
    {
        // Check for inputs if the abilities are available
        if (canWalk) HandleWalking();
        if (canRotate) HandleRotation();
        if (canScale) HandleScaling();

        // Continuously check if buttons need to be reset to their original position
        CheckAndResetButtonPosition();
    }

    // Methods for handling movement, rotation, scaling
    void HandleWalking()
    {
        float move = Input.GetAxisRaw("Horizontal");
        if (move != 0)
        {
            transform.Translate(Vector3.right * move * movespeed * Time.deltaTime);
        }
    }

    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotatespeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back * rotatespeed * Time.deltaTime);
        }
    }

    void HandleScaling()
    {
        if (Input.GetKey(KeyCode.S))
        {
            transform.localScale += Vector3.one * scalespeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.localScale -= Vector3.one * scalespeed * Time.deltaTime;
        }
    }

    // Modify abilities based on the button being dragged into the game
    public void SetAbility(Ability ability, bool isEnabled)
    {
        switch (ability)
        {
            case Ability.Walk:
                canWalk = isEnabled;
                break;
            case Ability.Rotate:
                canRotate = isEnabled;
                break;
            case Ability.Scale:
                canScale = isEnabled;
                break;
            default:
                break;
        }
    }

    // When the button is returned to the UI, restore all abilities
    public void ResetAbilities()
    {
        canWalk = true;
        canRotate = true;
        canScale = true;
    }

    // Check if buttons should return to their original positions
    private void CheckAndResetButtonPosition()
    {
        // If buttons are dragged back to the UI, reset their abilities
        if (IsButtonOutOfGameArea(walkButton))
        {
            walkButton.transform.position = originalWalkPos;
            SetAbility(Ability.Walk, true); // Restore walking ability
        }
        if (IsButtonOutOfGameArea(rotateButton))
        {
            rotateButton.transform.position = originalRotatePos;
            SetAbility(Ability.Rotate, true); // Restore rotating ability
        }
        if (IsButtonOutOfGameArea(scaleButton))
        {
            scaleButton.transform.position = originalScalePos;
            SetAbility(Ability.Scale, true); // Restore scaling ability
        }
    }

    // Helper method to check if the button is outside of the game area
    private bool IsButtonOutOfGameArea(Button button)
    {
        // Replace this logic with your own game area's bounds
        // You can check if the button is inside or outside certain screen limits
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        return rectTransform.anchoredPosition.y < 0; // Example: Check if button is below the screen
    }
}
