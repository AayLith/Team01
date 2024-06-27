using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulationSlider : MonoBehaviour
{
    private Slider popSlider;
    [SerializeField] private GameObject alive;
    [SerializeField] private GameObject dead;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private Transform tophat;
    private Animator sliderAnim;
    [SerializeField] private bool enableDebug = true;
    public int limit = 10;

    float sliderHeight;

    private void Start ()
    {
        popSlider = GetComponent<Slider> ();
        sliderAnim = GetComponent<Animator> ();
        sliderHeight = popSlider.GetComponent<RectTransform> ().sizeDelta.y * 0.98f;
    }


    private void Update ()
    {
        //for testing
        if ( enableDebug )
        {
            if ( Input.GetKeyDown ( KeyCode.X ) )
            {
                Debug.Log ( "Goblins Culled" );
                popSlider.value = popSlider.value - 10f;
            }
            if ( Input.GetKeyDown ( KeyCode.C ) )
            {
                Debug.Log ( "Goblins Spawned" );
                popSlider.value = popSlider.value + 10f;
            }
        }

        if ( tophat )
            tophat.localPosition = popSlider.fillRect.localPosition - Vector3.up * sliderHeight / 2 + Vector3.up * sliderHeight * ( popSlider.value / 100 );

        //ui percent text
        percentText.text = popSlider.value.ToString ( "f0" );


        //event triggers
        if ( popSlider.value == 0 )
        {
            //Debug.Log ( "GAME OVER" );
        }

        if ( popSlider.value < limit )
        {
            //Debug.Log ( "DANGER!" );
            sliderAnim.SetTrigger ( "PopulationDanger" );
        }
        else
        {
            sliderAnim.ResetTrigger ( "PopulationDanger" );
        }


        //trigger alive or dead image
        if ( popSlider.value <= limit )
        {
            dead.SetActive ( true );
            alive.SetActive ( false );
        }
        else
        {
            alive.SetActive ( true );
            dead.SetActive ( false );
        }
    }
}
