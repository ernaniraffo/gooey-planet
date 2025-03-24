using UnityEngine;

public class GameSingleton : MonoBehaviour
{
    public static GameSingleton instance;
    
    // manager instances
    public Player player {get; private set;}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInChildren<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
