using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AbilityFullZone : Ability
{
    [Header ( "Battle" )]
    [HideInInspector] public int targetRange;

    public override void setTargets ( List<BattleZone> allyZones , List<BattleZone> ennemyZones , Creature _caster = null )
    {
        // Reset
        targets.Clear ();

        // Init
        List<BattleZone> _targets = new List<BattleZone> ();

        // Gather all units in range
        foreach ( BattleZone opz in ennemyZones )
            if ( caster.zone.range + opz.range - 1 <= range )
                _targets.Add ( opz );

        if ( _targets.Count == 0 )
            return;

        // Choose a target at random
        _targets = _targets.shuffle ();
        targets = _targets[ 0 ].creatures;
        targetRange = caster.zone.range + _targets[ 0 ].range - 1;
    }

    protected override IEnumerator animationSecondPart ()
    {
        yield return StartCoroutine ( attackWindown ( targets[ 0 ].transform.position ) );

        foreach ( Creature c in targets )
        {
            c.getHit ();
            // Damages + Effetcs
            c.takeDamages ( damages );
        }

        yield return new WaitForSeconds ( 0.3f );
    }

    public override string ToString ()
    {
        string res = "";
        res += displayName + '\n';
        res += triggerToString ();
        res += "deals " + "<color=orange>" + damages + "</color>" + " to all ennemies in a zone at range " + "<color=yellow>" + range + "</color>" + '.' + '\n';
        return res;
    }
}
