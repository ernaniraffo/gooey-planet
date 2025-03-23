using UnityEngine;

public class Dog : MonoBehaviour
{
    RaycastHit hit;
    public Transform raycastOriginTransform;
    public float maxDistance;
    private Vector3 raycastOrigin;
    private bool noticedPlayer;
    public float dogWalkSpeed;
    public float health;
    public float maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        raycastOrigin = raycastOriginTransform.position;
        LookoutForPlayer();
        if (noticedPlayer) {
            // look at the player
            LookAtPlayer();
            // move to attack player
            MoveTowardsPlayer();
        }
    }

    private void LookoutForPlayer() {
        if (Physics.Raycast(raycastOrigin, transform.forward, out hit, maxDistance)) {
            // Debug.DrawRay(raycastOrigin, transform.forward, Color.blue, maxDistance);
            if (hit.transform.gameObject.CompareTag("Player")) {
                noticedPlayer = true;
                return;
            }
            noticedPlayer = false;
        }
    }

    private void LookAtPlayer() {
        // Debug.DrawRay(raycastOrigin, transform.forward, Color.green, maxDistance);
        transform.LookAt(hit.transform.position);
    }

    private void MoveTowardsPlayer() {
        // get the difference in direction
        Vector3 direction = hit.transform.position - transform.position;
        // multiply the direction by the walk speed of the dog
        direction = direction.normalized * dogWalkSpeed * Time.deltaTime;
        // move towards the player
        transform.position += new Vector3(direction.x, 0, direction.z);
    }

    public void DecrementHealth(int val) {
        health -= val;
    }

    public float GetHealthAsPercent() {
        return health / maxHealth;
    }
}