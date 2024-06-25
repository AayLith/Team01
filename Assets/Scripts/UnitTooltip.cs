using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitTooltip : MonoBehaviour
{
    public static UnitTooltip instance;

    public TMP_Text label;
    public Image image;
    public TMP_Text description;
    public TMP_Text abilities;
    public GameObject obj;

    private void Awake ()
    {
        instance = this;
        obj.SetActive ( value: false );
    }

    public void setToolTip ( Creature c )
    {
        label.text = c.displayName;
        image.sprite = c.spriteRenderer.sprite;
        description.text = c.description;
        abilities.text = c.abilitiesToString ();
        obj.SetActive ( true );
        //StartCoroutine ( updateSprite ( c ) );
    }

    public void hide ()
    {
        obj.SetActive ( value: false );
        //StopAllCoroutines ();
    }

    IEnumerator updateSprite ( Creature c )
    {
        while ( true )
        {
            image.sprite = c.spriteRenderer.sprite;
            yield return null;
        }
    }
}
