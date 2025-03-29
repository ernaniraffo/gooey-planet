using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dog : MonoBehaviour
{
    RaycastHit hit;
    public Transform raycastOriginTransform;
    public float maxDistance;
    private Vector3 raycastOrigin;
    private bool noticedPlayer;
    public float dogWalkSpeed;
    public float dogBiteDashDuration;
    private int health;
    private int maxHealth;
    private int healthDecrementWhenHitByWater;
    public int minDistanceToBitePlayer;
    private bool bitingPlayer;
    private bool didBitePlayer;
    
    // get all the goo on the animal
    public List<GooOnAnimal> gooOnDog;

    // points to walk to
    private List<Vector3> pointsToWalkTo;
    int homeIndex = 0;
    int currentWalkPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gooOnDog = new List<GooOnAnimal>(GetComponentsInChildren<GooOnAnimal>());
        // max health is the amount of goo on the dog * 2 since it requires two hits to delete one goo
        maxHealth = gooOnDog.Count * 2;
        // health is max health at first
        health = maxHealth;
        // since it requires two shots to delete the goo, the decrement will be 1.
        healthDecrementWhenHitByWater = 1;
        DebugPrintHealth();
        
        // set the points in which to walk to while idle
        SetPointsToWalkTo();
        currentWalkPoint = homeIndex + 1;
    }

    // Update is called once per frame
    void Update()
    {
        raycastOrigin = raycastOriginTransform.position;
        noticedPlayer = false;
        if (IsEvil()) {
            LookoutForPlayer();
            if (noticedPlayer) {
                // check if dog is already performing biting animation
                if (!IsBiting()) {
                    // the player could have notified this script that the dog performed a bite
                    SetBitPlayer(false);
                    // look at the player
                    LookAtPlayer();
                    // move to attack player
                    MoveTowardsPlayer();
                    // bite the player if within range
                    if (WithinBiteRange()) {
                        // play biting animation and set internal state
                        BitePlayer();
                    }
                }
            } else {
                // move to the next point
                if (ReachedPoint()) {
                    Debug.Log("Reached walk point");
                    currentWalkPoint = (currentWalkPoint + 1) % pointsToWalkTo.Count;
                    Debug.Log("New walk point: " + pointsToWalkTo[currentWalkPoint]);
                } else {
                    Debug.Log("Have not reached next point yet");
                }
                Debug.Log("Moving to point: " + pointsToWalkTo[currentWalkPoint]);
                MoveToPoint();
            }
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

    private void MoveToPoint() {
        // look at the next point
        transform.LookAt(pointsToWalkTo[currentWalkPoint]);
        // get the difference in direction
        Vector3 direction = pointsToWalkTo[currentWalkPoint] - transform.position;
        // multiply the direction by the walk speed of the dog
        direction = direction.normalized * dogWalkSpeed * Time.deltaTime;
        // move towards the player
        transform.position += new Vector3(direction.x, 0, direction.z);
    }

    public void DecrementHealth(int val) {
        health -= val;
        // DebugPrintHealth();
    }

    public float GetHealthAsPercent() {
        // Debug.Log("GetHealthASPercent: " + health + " / " + maxHealth + " = " + ((float) health / maxHealth));
        return (float) health / maxHealth;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water")) {
            Debug.Log("Water hit DOG!");
            // decrement the health of the first goo on the dog
            if (gooOnDog.Count > 0) {
                gooOnDog[0].DecrementGooAmount();
                DecrementHealth(healthDecrementWhenHitByWater);
                if (gooOnDog[0].IsCleaned()) {
                    Destroy(gooOnDog[0].gameObject);
                    gooOnDog.RemoveAt(0);
                }
            }
        }
    }

    private void DebugPrintHealth() {
        Debug.Log("\ndogMaxHealth=" + maxHealth + "\nhealth=" + health + "\nhealthDecrement=" + healthDecrementWhenHitByWater);
    }

    private bool IsEvil() {
        return gooOnDog.Count > 0;
    }

    private bool WithinBiteRange() {
        return (GameSingleton.instance.player.transform.position - transform.position).magnitude <= minDistanceToBitePlayer;
    }

    private void BitePlayer() {
        // set the bite state to true
        EnableBiting();
        // move the transform using DG Tweening
        Vector3 direction = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
        transform.DOMove(direction, dogBiteDashDuration).OnComplete(DisableBiting);
    }

    private void DisableBiting() {
        Debug.Log("Done biting player");
        bitingPlayer = false;
    }

    private void EnableBiting() {
        Debug.Log("Biting player");
        bitingPlayer = true;
    }

    public bool IsBiting() {
        return bitingPlayer;
    }

    public bool DidBitePlayer() {
        return didBitePlayer;
    }

    public void SetBitPlayer(bool bitePerformed) {
        didBitePlayer = bitePerformed;
    }

    private void SetPointsToWalkTo() {
        // this method should only be referenced once by the Start() method
        // make a triangular area where the dog will walk
        int metersToWalk = 5;
        pointsToWalkTo = new List<Vector3>
        {
            transform.position,
            transform.position + (Vector3.forward * metersToWalk),
            transform.position + (Vector3.right * metersToWalk)
        };
        Debug.Log("Points to walk to: " + pointsToWalkTo);
    }

    private bool ReachedPoint() {
        return (pointsToWalkTo[currentWalkPoint] - transform.position).magnitude < 1;
    }
}