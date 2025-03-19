using UnityEngine;

public class Goo : MonoBehaviour
{
    private Renderer gooRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gooRenderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water")) {
            Color currentColor = gooRenderer.material.color;
            gooRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a - 0.05f);
            Debug.Log("Alpha: " + currentColor.a);
            if (gooRenderer.material.color.a <= 0.90f) {
                Destroy(gameObject);
            }
        }
    }
}
