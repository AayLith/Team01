using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellWeakness : Spell
{
    public int amount;

    protected override bool costs ()
    {
        throw new System.NotImplementedException ();
    }

    protected override List<Creature> getTargetCreatures ()
    {
        throw new System.NotImplementedException ();
    }

    protected override Vector3 getTargetPos ()
    {
        throw new System.NotImplementedException ();
    }

    protected override void execute ( Creature c )
    {
        throw new System.NotImplementedException ();
    }
}
