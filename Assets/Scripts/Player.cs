using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region INPUTS
    Vector2 moveInput;
    bool attackInput;
    #endregion

    #region COMPONENTS
    CharacterController characterController;
    #endregion

    #region PLAYER PROPERTIES
    private float playerSpeed = 10f;
    private float waterDropLaunchCoolDown = 0.025f;
    private float timeSinceLastWaterDropLaunch = 0;
    private Vector3 playerVelocity;
    private float gravityValue = Physics.gravity.y;
    private float gravityFallingMultiplier = 3;
    private float maxFallSpeed = 10;
    private float rotationSpeed = 15.0f;
    private int health;
    #endregion

    #region REFERENCE OBJECTS
    public GameObject hoseEnd;
    public GameObject waterDrop;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the components
        characterController = GetComponent<CharacterController>();

        // start with base health
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        HandleVerticalVelocity();
        Move();
        if (Time.time - timeSinceLastWaterDropLaunch >= waterDropLaunchCoolDown) {
            timeSinceLastWaterDropLaunch = 0;
        }
        if (attackInput) {
            SplashWater();
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context) {
        attackInput = context.ReadValueAsButton();
    }

    public void OnInteract(InputAction.CallbackContext context) {
        
    }

    private void Move() {
        // Get the movement direction
        Vector3 movementDirection = CalculateMovementDirection();
        // Rotate the player in the movement direction. x-axis is horizontal, z-axis is vertical.
        Rotate(movementDirection.x, movementDirection.z);
        // This is the last move call which will update the collision flags
        // It is recommended to only have one move method per frame since each call updates the
        // collision flags
        characterController.Move(movementDirection * Time.deltaTime);
        // if (IsIdle() || IsFalling() || IsJumping()) return;
        // // Set the walking animation
        // SetWalking(true);
        // Debug.Log("Movement direction: " + movementDirection + " ## isGrounded: " + characterController.isGrounded);
    }

    private Vector3 CalculateMovementDirection() {
        // Get the movement direction
        // Get the camera's forward and right vectors
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        // Create a movement direction vector from the camera's forward and right vector and the
        // player input
        Vector3 forward = moveInput.y * cameraForward;
        Vector3 right = moveInput.x * cameraRight;
        // Move the character in the movement direction with the player speed
        Vector3 movementDirection = (forward + right) * playerSpeed;
        // The Y axis is reserved for the gravity
        movementDirection.y = playerVelocity.y;
        return movementDirection; 
    }

    private void Rotate(float horizontal, float vertical) {
        if (Mathf.Abs(horizontal) < 0.05 && Mathf.Abs(vertical) < 0.05) {
            // don't rotate the character if it is not moving
            // removing this would cause the character to reset it's rotation when movement stops.
            // set walking animation false
            // SetWalking(false);
            // if (!IsJumping() || !IsFalling()) {
            //     // if we are not jumping and not moving and not falling, we are idle
            //     SetIdle(true);
            // }
            return;
        }
        // we cannot be idle if moving
        // SetIdle(false);
        // calculate the rotation
        float angle = Mathf.Rad2Deg * Mathf.Atan2(horizontal, vertical);
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
        // set the rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleVerticalVelocity() {
        // clamp player velocity
        if (characterController.isGrounded && playerVelocity.y < 0) {
            // Hitting this part of the code means we were jumping and are done.
            // Let's reset the jump animation
            Debug.Log("character is grounded now");
            playerVelocity.y = 0f;
            // coyoteFrames = 0;
            // SetJumping(false);
        }
        float gravityScale = gravityValue;
        // player has reached peak of jump and is starting to fall
        if (playerVelocity.y < 0) {
            // multiply gravity scale by multiplier for faster fall
            gravityScale *= gravityFallingMultiplier;
            Debug.Log("New gravity scale: " + gravityScale);
            // clamp the player velocity so our fall speed is consistent
            playerVelocity.y = Mathf.Max(playerVelocity.y, -maxFallSpeed);
        }
        // increase the player velocity by gravity scale to make player jump or fall
        playerVelocity.y += gravityScale * Time.deltaTime;
        Debug.Log("Player velocity: " + playerVelocity);

        // if (IsFalling()) {
        //     coyoteFrames++;
        // }
    }

    private void SplashWater() {
        if (timeSinceLastWaterDropLaunch == 0) {
            // spawn the water droplet in front of the hose end
            Vector3 positionToSpawn = hoseEnd.transform.position + (hoseEnd.transform.forward * 0.5f);
            // instantiating the water droplet will make it go forwards for some time
            Instantiate(waterDrop, positionToSpawn, hoseEnd.transform.rotation);
            timeSinceLastWaterDropLaunch = Time.time;
        }
    }

    #region PUBLIC METHODS
    public void DecrementHealth(int val) {
        health -= val;
    }

    public void IncrementHealth(int val) {
        health += val;
    }

    public int GetHealth() {
        return health;
    }
    #endregion
}
