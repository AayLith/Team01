using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTempest : Spell
{
    public int amount;

    protected override bool costs ()
    {
        return PlayerController.instance.removePopulation ( cost );
    }

    protected override List<Creature> getTargetCreatures ()
    {
        List<Creature> targets = new List<Creature> ();
        foreach ( BattleZone bz in BattleController.instance.opponentZones )
            targets.AddRange ( bz.creatures );
        return targets;
    }

    protected override Vector3 getTargetPos ()
    {
        Vector3 pos = Vector3.zero;
        foreach ( BattleZone bz in BattleController.instance.opponentZones )
            pos += bz.transform.position;
        return pos / BattleController.instance.opponentZones.Count;
    }

    protected override void execute ( Creature c )
    {
        c.takeDamages ( amount );
    }
}
