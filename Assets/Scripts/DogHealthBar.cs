using UnityEngine;
using UnityEngine.UI;

public class DogHealthBar : MonoBehaviour
{
    public Image dogHealthBar;
    public Dog dog;
    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       cam = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        dogHealthBar.fillAmount = dog.GetHealthAsPercent();
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
