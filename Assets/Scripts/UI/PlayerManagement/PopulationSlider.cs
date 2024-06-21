using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulationSlider : MonoBehaviour
{
    private Slider popSlider;
    [SerializeField] private GameObject alive;
    [SerializeField] private GameObject dead;
    [SerializeField] private TextMeshProUGUI percentText;
    private Animator sliderAnim;

    
    private void Start()
    {
        popSlider = GetComponent<Slider>();
        sliderAnim = GetComponent<Animator>();
    }


    private void Update()
    {
        //for testing
        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Goblins Culled");
            popSlider.value = popSlider.value - 10f;
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Goblins Spawned");
            popSlider.value = popSlider.value + 10f;
        }


        //ui percent text
        percentText.text = popSlider.value.ToString();


        //event triggers
        if (popSlider.value == 0)
        {
            Debug.Log("GAME OVER");
        }

        if(popSlider.value < 20)
        {
            Debug.Log("DANGER!");
            sliderAnim.SetTrigger("PopulationDanger");
        }


        //trigger alive or dead image
        if(popSlider.value > 0)
        {
            alive.SetActive(true);
            dead.SetActive(false);
        }
        else if(popSlider.value == 0 )
        {
            dead.SetActive(true);
            alive.SetActive(false);
        }

       

    }
}
