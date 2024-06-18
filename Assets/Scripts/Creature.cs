using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [TextArea ( 3 , 10 )]
    public string description;

    public SpriteRenderer spriteRenderer;
    [Min ( 1 )]
    public int health = 2;
    public int curhealth = 2;
    [Range ( 0 , 10 )]
    public int meleeDmg = 1;
    [Range ( 0 , 10 )]
    public int rangedDmg = 0;
    [Range ( 1 , 5 )]
    public int atkRange = 1;
    public Ability[] abilities;

    [Header ( "Battle" )]
    public int preferedRange = 1;
    public Healthbar healthbar;
    public int healthWidth = 50;
    public int healthHeight = 7;
    public int healthOffset = -10;
    [HideInInspector]
    public Creature target;
    public int targetRange;
    public BattleZone zone;
    public bool isdying = false;

    [Header ( "Animations" )]
    public GameObject attackCast;
    public GameObject attackProjectile;
    public float projectileSpeed = 100;
    public GameObject attackHit;
    public GameObject selfhit;
    public float attackDamageDelay = 0;

    private void Update ()
    {
        healthbar.updatePos ( this );
    }

    private void Awake ()
    {
        healthbar.init ( curhealth , health , healthWidth , healthHeight , healthOffset );
    }

    public void takeDamages ( int amount )
    {
        curhealth -= amount;
        healthbar.updateValue ( curhealth );
    }
}
