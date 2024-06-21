using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    private AudioSource buttonHover;
    private AudioSource buttonClick;


    private void Start()
    {
        buttonHover = gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        buttonClick = gameObject.transform.GetChild(1).GetComponent<AudioSource>();
    }


    public void OnButtonHover()
    {
        buttonHover.Play();
    }

    public void OnButtonClick()
    {
        buttonClick.Play();
    }

}
