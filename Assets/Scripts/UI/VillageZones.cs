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
        textImage.SetActive(true);
        zoneName.SetActive(true);
    }

    public void SetUIElementsOff()
    {
        backgroundImage.SetActive(false);
        textImage.SetActive(false);
        zoneName.SetActive(false);
    }
}
