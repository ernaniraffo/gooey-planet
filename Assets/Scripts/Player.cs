using UnityEngine;

public class Player : MonoBehaviour
{
    #region INPUTS
    Vector2 moveInput;
    #endregion

    #region COMPONENTS
    CharacterController characterController;
    #endregion

    #region PLAYER PROPERTIES
    private float playerSpeed = 5f;
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
        
    }

    void Move() {
        // Move the character according to input
        Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.Move(movementDirection * playerSpeed * Time.deltaTime);
    }    
}
