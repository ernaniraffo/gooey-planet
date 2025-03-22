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
    }

    // Update is called once per frame
    void Update()
    {
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
        // Move the character according to input
        Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        movementDirection *= playerSpeed;
        characterController.Move(movementDirection * Time.deltaTime);
    }
   
    private void SplashWater() {
        if (timeSinceLastWaterDropLaunch == 0) {
            // spawn the water droplet in front of the hose end
            Vector3 positionToSpawn = hoseEnd.transform.position + new Vector3(0, 0, hoseEnd.transform.lossyScale.z / 2 + 1);
            // instantiating the water droplet will make it go forwards for some time
            Instantiate(waterDrop, positionToSpawn, Quaternion.identity);
            timeSinceLastWaterDropLaunch = Time.time;
        }
    }
}
