using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Spell spell;

    private void Awake ()
    {
        spell = GetComponent<Spell> ();
    }

    public void useSpell ()
    {
        spell.useSpell ();
    }

    public void OnPointerEnter ( PointerEventData eventData )
    {
        SpellTooltip.instance.open ( spell );
    }

    public void OnPointerExit ( PointerEventData eventData )
    {
        SpellTooltip.instance.close ();
    }
}
