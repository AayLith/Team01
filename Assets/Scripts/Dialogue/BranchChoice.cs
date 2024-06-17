using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine.UI;
using TMPro;

public class BranchChoice : MonoBehaviour
{
    private Branch branch;
    private ArticyFlowPlayer flowPlayer;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] private bool diagSkip = false;
    [SerializeField] public GameObject diagManager;
    [SerializeField] public GameObject nextButton;

    void Awake()
    {
        diagManager = GameObject.Find("DialogueManager");
        nextButton = GameObject.Find("DialogueBranches");
    }

    void Update()
    {
        var script = diagManager.GetComponent<DialogueManager>();
        var button = nextButton.transform.GetChild(0);

        if (Input.GetKeyDown(KeyCode.Space) && diagSkip == true && script.diagActive == false)
        {
                OnBranchSelected();
        }

        if(script.diagActive == true)
        {
            button.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        } 
    }

    public void TaskOnClick()
    {
        var script = diagManager.GetComponent<DialogueManager>();

        script.diagFast = false; 
    }

    public void AssignBranch(ArticyFlowPlayer aFlowPlayer, Branch aBranch)
    {
        branch = aBranch;
        flowPlayer = aFlowPlayer;
        IFlowObject target = aBranch.Target;
        buttonText.text = string.Empty;

        var objectWithMenuText = target as IObjectWithMenuText; 
        if(objectWithMenuText != null)
        {
            buttonText.text = objectWithMenuText.MenuText;
        }

        if(string.IsNullOrEmpty(buttonText.text))
        {
            buttonText.text = ">>>";
            diagSkip = true;
        }
    }

    public void OnBranchSelected()
    {
        var script = diagManager.GetComponent<DialogueManager>();
        script.diagFast = false;

        if (script.diagActive == false)
        {
            flowPlayer.Play(branch);
        }
    }
}
