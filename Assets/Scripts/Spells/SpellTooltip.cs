using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellTooltip : MonoBehaviour
{
    public static SpellTooltip instance;

    public GameObject target;
    public TMP_Text nameText;
    public TMP_Text destricptionText;
    public TMP_Text flavorText;

    private void Awake ()
    {
        instance = this;
        target.SetActive ( false );
    }

    public void open(Spell s )
    {
        target.SetActive ( true );
        nameText.text = s.displayName;
        destricptionText.text = s.description;
        flavorText.text = s.flavor;
    }

    public void close ()
    {
        target.SetActive ( false );
    }
}
