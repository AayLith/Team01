using UnityEngine;
using Articy.Unity;
using Articy.Goblinkingarticytest.GlobalVariables;

public class cutscene1 : MonoBehaviour
{

    [Header("Dialogue Variables")]
    private DialogueManager dialogueManager;
    public GameObject dialogueWidget;
    private ArticyObject availableDialogue;
    private AudioSource dialogueSound;
    private bool dialogueIsActive;


    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        availableDialogue = GetComponent<ArticyReference>().reference.GetObject();

        StartCutscene();
    }


    private void StartCutscene()
    {
        dialogueManager.StartDialogue(availableDialogue);
    }

}
