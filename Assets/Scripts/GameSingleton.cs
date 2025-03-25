using UnityEngine;

public class GameSingleton : MonoBehaviour
{
    public static GameSingleton instance;
    
    // manager instances
    public Player player {get; private set;}

    void Awake()
    {
        // assign the instance if it is not yet assigned
        // otherwise the instance is coming from another scene, so destroy me
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

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
