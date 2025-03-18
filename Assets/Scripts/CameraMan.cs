using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public GameObject player;
    public Camera mainCamera;

    // how far from the player do we want to be
    private float zOffset = -10; // 5 meters behind player
    private float yOffset = 2; // 2 meters above player
    private float xOffset = 2; // be a little bit to the left of player
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;            
    }

    // Update is called once per frame
    void Update()
    {
        // Always follow the player
        Vector3 playerPos = player.transform.position;
        // Let's be some meters behind the player
        mainCamera.transform.position = playerPos + new Vector3(xOffset, yOffset, zOffset);     
    }
}
