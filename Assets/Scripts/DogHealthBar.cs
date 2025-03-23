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
        Debug.Log("dogHealthBarValue: " + dogHealthBar.fillAmount);
        // make sure the health bar doesn't disappear even if dog is onie
        if (dogHealthBar.fillAmount > 0 && dogHealthBar.fillAmount < 0.1) {
            dogHealthBar.fillAmount = 0.1f;
        }
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
