using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellConfirm : MonoBehaviour
{
    public static SpellConfirm instance;

    public GameObject target;
    public bool valid = false;
    public bool done = false;
    public TMP_Text spellName;

    private void Awake ()
    {
        instance = this;
        valid = false;
        done = false;
        target.SetActive ( false );
    }

    public void open ( Spell s )
    {
        target.SetActive ( true );
        spellName.text = s.displayName;
        valid = false;
        done = false;
    }

    public void validate ( bool val )
    {
        valid = val;
        target.SetActive ( false );
        done = true;
    }
}
