using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorChange : MonoBehaviour
{
    [SerializeField] private Slider popSlider;
    private Image backgroundImage;


    private void Start()
    {
        backgroundImage = GetComponent<Image>();
    }


    private void Update()
    {
        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 
            10f/popSlider.value);

       
    }
}
