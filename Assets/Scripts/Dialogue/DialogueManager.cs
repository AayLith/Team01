using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Goblinkingarticytest;

public class DialogueManager : MonoBehaviour, IArticyFlowPlayerCallbacks
{

    [Header("UI")]
    [SerializeField] GameObject dialogueWidget;
    [SerializeField] public TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI dialogueSpeaker;
    [SerializeField] RectTransform branchLayoutPanel;
    [SerializeField] GameObject branchPrefab;
    [SerializeField] GameObject closePrefab;
    [SerializeField] Image icon1;
    [SerializeField] Image icon2;
    [SerializeField] AudioSource diagSound;


    //is the ui interface on?
    public bool DialogueActive { get; set; }
    [SerializeField] public bool diagFast = false;
    [SerializeField] public bool diagActive = false;

 

    private ArticyFlowPlayer flowPlayer;

    void Start()
    {
        flowPlayer = GetComponent<ArticyFlowPlayer>();
    }
    
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Space) && diagFast == false && dialogueWidget.activeInHierarchy == true)
            {
                diagFast = true;
            }
    }

    public void StartDialogue(IArticyObject aObject)
    {
        DialogueActive = true;
        dialogueWidget.SetActive(DialogueActive);
        flowPlayer.StartOn = aObject;
    }


    public void CloseDialogueBox()
    {
        DialogueActive = false;
        dialogueWidget.SetActive(DialogueActive);
        flowPlayer.FinishCurrentPausedObject();
    }


    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        dialogueText.text = string.Empty;
        dialogueSpeaker.text = string.Empty;
        diagFast = false;

        var objectWithText = aObject as IObjectWithText;
        if(objectWithText != null)
        {
            
            StartCoroutine(TypeSentence(objectWithText.Text));
        }

        var objectWithSpeaker = aObject as IObjectWithSpeaker;
        if(objectWithSpeaker != null)
        {
            var speakerEntity = objectWithSpeaker.Speaker as Entity;
            if(speakerEntity != null)
            {
                dialogueSpeaker.text = speakerEntity.DisplayName;
            }
        }

        var objectWithStageDirections = aObject as IObjectWithStageDirections;
        if(objectWithStageDirections !=null)
        {
            var speakerDirections = objectWithStageDirections.StageDirections;
            if(speakerDirections == "1")
            {
                icon1.gameObject.SetActive(true);
                icon2.gameObject.SetActive(false);
            }
            else if(speakerDirections == "2")
            {
                icon2.gameObject.SetActive(true);
                icon1.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        yield return new WaitForSeconds(1);
        foreach (char letter in sentence.ToCharArray())
        {
           if(diagFast == true)
           {
                dialogueText.text = "";
                diagSound.Play();
                dialogueText.text = sentence;
                diagActive = false;
           }
           else
           {
                diagActive = true;
                dialogueText.text += letter;
                diagSound.Play();
                yield return new WaitForSeconds(0.025f);
                diagActive = false;
           }
        }
    }

    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        ClearAllBraches();

        bool dialogueIsFinished = true;
        foreach(var branch in aBranches)
        {
            if(branch.Target is IDialogueFragment)
            {
                dialogueIsFinished = false;
            }
        }

        if(!dialogueIsFinished)
        {
            foreach(var branch in aBranches)
            {
                GameObject btn = Instantiate(branchPrefab, branchLayoutPanel);
                btn.GetComponent<BranchChoice>().AssignBranch(flowPlayer, branch);
            }
            
        }
        else
        {
            GameObject btn = Instantiate(closePrefab, branchLayoutPanel);
            var btnComp = btn.GetComponent<Button>();
            btnComp.onClick.AddListener(CloseDialogueBox);
        }
    }

    void ClearAllBraches()
    {
        foreach (Transform child in branchLayoutPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
