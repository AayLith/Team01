using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header ( "Autobuilder" )]
    public int cost;
    public int incrementalCost;
    public bool boss = false;

    [Header ("Stats")]
    [TextArea ( 3 , 10 )]
    public string description;
    [Min ( 1 )]
    public int health = 2;
    public int curhealth = 2;
    public Ability attack;
    public Ability[] abilities;
    public Synergy synergy;

    [Header("GameObjects")]
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteAnim;
    public Healthbar healthbar;

    [Header ( "Battle" )]
    public int preferedRange = 1;
    public int healthWidth = 50;
    public int healthHeight = 7;
    public int healthOffset = -10;
    public BattleZone zone;
    public bool isdying = false;

    private void Update ()
    {
        healthbar.updatePos ( this );
    }

    private void Awake ()
    {
        healthbar.init ( curhealth , health , healthWidth , healthHeight , healthOffset );
        Destroy ( spriteRenderer.gameObject );
        spriteRenderer = Instantiate ( spriteAnim.gameObject , transform.position , Quaternion.Euler ( -90 , 0 , 0 ) , transform ).GetComponent<SpriteRenderer> ();
        if ( zone )
            spriteRenderer.transform.localScale = Vector3.Scale ( spriteRenderer.transform.localScale , new Vector3 ( zone.X_Scale , 1 , 1 ) );
        for ( int i = 0 ; i < abilities.Length ; i++ )
        {
            abilities[ i ] = Instantiate ( abilities[ i ] , transform );
            abilities[ i ].caster = this;
        }
        if ( attack )
        {
            attack = Instantiate ( attack , transform );
            attack.caster = this;
        }
    }

    public void takeDamages ( int amount )
    {
        curhealth -= amount;
        healthbar.updateValue ( curhealth );
    }

    public void getHit ()
    {
        StartCoroutine ( hitAnim () );
    }

    IEnumerator hitAnim ()
    {
        yield return StartCoroutine ( animDash ( 5 , 0.025f ) );
        yield return StartCoroutine ( animDash ( 5 , -0.025f ) );
    }

    public IEnumerator animDash ( int steps , float distance )
    {
        for ( int i = 0 ; i < steps ; i++ )
        {
            spriteRenderer.transform.localPosition += new Vector3 ( distance / steps , 0 , 0 ) * -spriteRenderer.transform.localScale.x;
            yield return null;
        }
    }
}
