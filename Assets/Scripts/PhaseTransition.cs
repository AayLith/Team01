using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhaseTransition : MonoBehaviour
{
    public static PhaseTransition instance;

    public GameObject transitionAnim;
    public TMP_Text phaseText;
    public TMP_Text helpText;

    private void Awake ()
    {
        instance = this;
    }

    public void setText ( string p , string h )
    {
        phaseText.text = p;
        helpText.text = h;
    }

    public void transition ( PhaseMenu from , PhaseMenu to )
    {
        StartCoroutine ( transitionCo ( from , to ) );
    }

    IEnumerator transitionCo ( PhaseMenu from , PhaseMenu to )
    {
        Instantiate ( transitionAnim , Vector3.zero , Quaternion.identity , transform );
        yield return new WaitForSeconds ( 5.25f );
        from.close ();
        to.open ();
    }
}
