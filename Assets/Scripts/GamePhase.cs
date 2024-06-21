using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePhase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI phaseText;


    private void Update()
    {
        //testing
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Management Phase");
            phaseText.text = "Management Phase";
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Battle Phase");
            phaseText.text = "Battle Phase";
        }

    }
}
