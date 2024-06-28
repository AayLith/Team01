using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VillageZones : MonoBehaviour
{
    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private GameObject textImage;
    [SerializeField] private GameObject zoneName;


    public void SetUIElementsOn()
    {
        backgroundImage.SetActive(true);
        //textImage.SetActive(true);
        textImage.GetComponent<RectTransform> ().localScale = Vector3.one * 1.1f;
        //zoneName.SetActive(true);
    }

    public void SetUIElementsOff()
    {
        backgroundImage.SetActive(false);
        //textImage.SetActive(false);
        textImage.GetComponent<RectTransform> ().localScale = Vector3.one;
        //zoneName.SetActive(false);
    }
}
