using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider playerHealthSlider;
    public Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update the player health
        UpdatePlayerHealth();
    }

    private void UpdatePlayerHealth() {
        playerHealthSlider.value = player.GetHealth();
    }
}
