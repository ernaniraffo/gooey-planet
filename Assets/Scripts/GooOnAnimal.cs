using UnityEngine;

public class GooOnAnimal : MonoBehaviour
{
    private Renderer gooRenderer;
    private bool cleaned = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gooRenderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecrementGooAmount() {
        Color currentColor = gooRenderer.material.color;
        gooRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a - 0.05f);
        Debug.Log("Alpha: " + currentColor.a);
        if (gooRenderer.material.color.a <= 0.90f) {
            cleaned = true;
        }
    }

    public bool IsCleaned() {
        return cleaned;
    }
}
