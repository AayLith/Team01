using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    public static SpellCasting instance;

    public GameObject target;
    public TMP_Text nameText;

    private void Awake ()
    {
        instance = this;
        target.SetActive ( false );
    }

    public void open ( Spell s )
    {
        target.SetActive ( true );
        nameText.text = "Casting " + s.displayName + "..." + '\n' 
            + "Click on a zone to target." + '\n' 
            + "Right Click to cancel.";
    }

    public void close ()
    {
        target.SetActive ( false );
    }
}
