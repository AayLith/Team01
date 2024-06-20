using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AbilityAoE : Ability
{
    [Range ( 1 , 10 )]
    public int secondaryTargets = 1;
    [HideInInspector] public int targetRange;

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

        // Then choose an number of secondaryTarget in the same zone.
        _targets.Remove ( targets[ 0 ] );
        _targets.shuffle ();

        for ( int i = 0 ; i < secondaryTargets && i < _targets.Count ; i++ )
            targets.Add ( _targets[ i ] );
    }

    protected override IEnumerator animationSecondPart ()
    {
        yield return StartCoroutine ( attackWindown ( targets[ 0 ].transform.position ) );

        foreach ( Creature c in targets )
        {
            c.getHit ();
            // Damages + Effetcs
            c.takeDamages ( rangedDmg );
        }

        yield return new WaitForSeconds ( 0.3f );
    }
}
