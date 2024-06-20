using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMultiHit : Ability
{
    [Header ( "Battle" )]
    [Range ( 1 , 10 )]
    public int multiAttacks = 1;
    [Range ( 0 , 10 )]
    public int multiHitDmg = 1;
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

        // Then choose an number of targets at random.
        _targets.shuffle ();

        for ( int i = 0 ; i < multiAttacks + 1 && i < _targets.Count ; i++ )
            targets.Add ( _targets[ i ] );
    }
}
