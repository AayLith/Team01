
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    public SpriteRenderer render;
    public Sprite[] sprites;
    public float speed = 10;
    public int repeats = 1;

    private void Awake ()
    {
        StartCoroutine ( anim () );
    }

    IEnumerator anim ()
    {
        for ( int i = 0 ; i < repeats ; i++ )
        {
            for ( int j = 0 ; j < sprites.Length ; j++ )
            {
                render.sprite = sprites[ j ];
                yield return new WaitForSeconds ( 1 / speed );
            }
        }
        Destroy ( gameObject );
    }
}
