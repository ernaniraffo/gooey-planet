using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    RaycastHit hit;
    public Transform raycastOriginTransform;
    public float maxDistance;
    private Vector3 raycastOrigin;
    private bool noticedPlayer;
    public float dogWalkSpeed;
    private int health;
    private int maxHealth;
    private int healthDecrementWhenHitByWater;
    
    // get all the goo on the animal
    public List<GooOnAnimal> gooOnDog;

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
    }

    // Update is called once per frame
    void Update()
    {
        raycastOrigin = raycastOriginTransform.position;
        LookoutForPlayer();
        if (IsEvil() && noticedPlayer) {
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
        DebugPrintHealth();
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
}