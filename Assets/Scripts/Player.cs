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
    private Vector3 gravityForce;
    private float gravityMultiplier = 3f;
    private float waterDropSpeed = 5f;
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

    private void Move() {
        // Move the character according to input
        Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        movementDirection *= playerSpeed;
        characterController.Move(movementDirection * Time.deltaTime);
        Debug.Log("Move input: " + moveInput);
    }

    private void SplashWater() {
        StartCoroutine(SpawnWaterDrop());
    }

    IEnumerator SpawnWaterDrop() {
        float timeALive = 0;
        float maxTimeAlive = 4f;
        // spawn the water droplet in front of the hose end
        Vector3 positionToSpawn = hoseEnd.transform.position + new Vector3(0, 0, hoseEnd.transform.lossyScale.z / 2);
        // instantiating the water droplet will make it go forwards for some time
        GameObject waterDropSpawned = Instantiate(waterDrop, positionToSpawn, Quaternion.identity, hoseEnd.transform);
        // get the rigid body to add force to
        Rigidbody waterDropRigidBody = waterDropSpawned.GetComponent<Rigidbody>();        
        // get current player's forward direction when coroutine starts        
        Vector3 forwardDirectionAtTimeOfSplash = transform.forward;

        while (timeALive < maxTimeAlive) {
            timeALive += Time.deltaTime;
            waterDropRigidBody.AddForce(forwardDirectionAtTimeOfSplash * waterDropSpeed);
            yield return null;
        }

        // destroy the water droplet once done
        Destroy(waterDropSpawned);
    }
}
