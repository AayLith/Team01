using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header ( "Army Building" )]
    [Range ( 1 , 100 )]
    public int cost;
    [Range ( 0 , 10 )]
    public int incrementalCost;
    [Range ( 0 , 10 )]
    public int popularity;
    public bool boss = false;

    [Header ( "Stats" )]
    [TextArea ( 3 , 10 )]
    public string description;
    public string displayName;
    [Min ( 1 )]
    public int health = 2;
    public int curhealth = 2;
    public Ability attack;
    public Ability[] abilities;
    public Synergy synergy;

    [Header ( "GameObjects" )]
    public Transform skin;
    public Animation animator;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteAnim;
    public Healthbar healthbar;
    //public Outline outline;

    [Header ( "Battle" )]
    [Range ( 1 , 3 )]
    public int preferedRange = 1;
    public int healthWidth = 50;
    public int healthHeight = 7;
    public int healthOffset = -10;
    [Range ( 1 , 100 )]
    public int loot = 0;
    public BattleZone zone;
    public bool isDead = false;

    private void Update ()
    {
        healthbar.updatePos ( this );
    }

    private void Awake ()
    {
        healthbar.init ( curhealth , health , healthWidth , healthHeight , healthOffset );
        Destroy ( spriteRenderer.gameObject );
        spriteRenderer = Instantiate ( spriteAnim.gameObject , transform.position , Quaternion.Euler ( -90 , 0 , 0 ) , skin ).GetComponent<SpriteRenderer> ();
        //outline = spriteRenderer.gameObject.AddComponent<Outline> ();
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

    public string abilitiesToString ()
    {
        string res = "";

        if ( synergy )
            res += "<size=120%>" + "<align=\"center\">" + synergy.name + '\n' + "</align>" + "</size>";

        if ( tag == "OpponentUnit" )
            res += "<size=120%>" + "<align=\"center\">" + "Loot : " + "<color=white>" + loot + "</color>" + '\n' + " </align>" + "</size>";

        res += "<size=120%>" + "<align=\"center\">" + "Health : " + "<color=green>" + curhealth + "</color> / <color=green>" + health + "</color>" + '\n' + "</align>" + "</size>";

        if ( attack )
            res += "<size=120%>" + "<align=\"center\">" + attack.ToString () + "</align>" + "</size>";

        foreach ( Ability a in abilities )
            res += a.ToString ();

        return res;
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
        //   yield return StartCoroutine ( animDash ( 5 , 0.025f ) );
        // yield return StartCoroutine ( animDash ( 5 , -0.025f ) );
        yield return null;
    }

    public IEnumerator animDash ( int steps , float distance )
    {
        for ( int i = 1 ; i < steps + 1 ; i++ )
        {
            spriteRenderer.transform.localPosition += new Vector3 ( distance / steps , 0 , 0 ) * -spriteRenderer.transform.localScale.x;
            yield return null;
        }
    }
}
