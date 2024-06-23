using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttack : Ability
{
    [Header ( "Battle" )]
    [HideInInspector] public int targetRange;
    [Range ( 0 , 10 )]
    public int meleeDmg = 1;

    public override void setTargets ( List<BattleZone> allyZones , List<BattleZone> ennemyZones , Creature _caster = null )
    {
        // Reset
        targets.Clear ();
        targetRange = 0;

        // Init
        List<Creature> _targets = new List<Creature> ();

        // Gather all units in range
        foreach ( BattleZone opz in ennemyZones )
            if ( caster.zone.range + opz.range - 1 <= range )
                _targets.AddRange ( opz.creatures );

        if ( _targets.Count == 0 )
            return;

        // Choose a target at random
        targets.Add ( _targets.getRandomElement () );
        targetRange = caster.zone.range + _targets[ 0 ].zone.range;
    }

    protected override IEnumerator animationFirstPart ()
    {
        if ( targets.Count == 0 )
            yield break;

        // Caster windup
        //yield return StartCoroutine ( caster.animDash ( 5 , 0.025f ) );
        caster.animator.Play ( "Windup" );
        while ( caster.animator.isPlaying )
            yield return null;

        // Cast anim
        if ( cast && targets[ 0 ].zone.range + caster.zone.range == 2 && range == 1 || cast && targets[ 0 ].zone.range + caster.zone.range > 2 && range > 1 )
        {
            GameObject castAnim = Instantiate ( cast , caster.transform.position + new Vector3 ( 0 , 0 , -0.05f ) , cast.transform.rotation );
            yield return StartCoroutine ( waitForDestruction ( castAnim ) );
        }

        // Caster jump
        //yield return StartCoroutine ( caster.animDash ( 5 , -0.075f ) );
        caster.animator.Play ( "Assault" );
        while ( caster.animator.isPlaying )
            yield return null;
        //StartCoroutine ( caster.animDash ( 5 , 0.05f ) );
        caster.animator.Play ( "Windown" );
    }

    protected override IEnumerator animationSecondPart ()
    {
        // For each target + Projectile
        foreach ( Creature target in targets )
        {
            yield return StartCoroutine ( attackWindown ( target.transform.position ) );
            target.getHit ();

            // Damages + Effetcs
            if ( target.zone.range + caster.zone.range == 2 )
                target.takeDamages ( meleeDmg );
            else
                target.takeDamages ( rangedDmg );

            yield return new WaitForSeconds ( 0.1f );
        }
        yield return new WaitForSeconds ( 0.2f );
    }
}
