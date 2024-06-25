using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpellHeal : Spell
{
    public int amount;

    protected override bool costs ()
    {
        return PlayerController.instance.removePopulation ( cost );
    }

    protected override List<Creature> getTargetCreatures ()
    {
        List<Creature> targets = new List<Creature> ();
        targets.AddRange ( BattleController.instance.playerReserve.creatures );
        foreach ( BattleZone bz in BattleController.instance.playerZones )
            targets.AddRange ( bz.creatures );
        return targets;
    }

    protected override Vector3 getTargetPos ()
    {
        Vector3 pos = Vector3.zero;
        foreach ( BattleZone bz in BattleController.instance.playerZones )
            pos += bz.transform.position;
        return pos / BattleController.instance.playerZones.Count;
    }

    protected override void execute ( Creature c )
    {
        c.health += amount;
        c.animator.Play ( "Ability" );
    }
}
