using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFireball : Spell
{
    public int amount;
    BattleZone targetZone;

    protected override IEnumerator targeting ()
    {
        SpellCasting.instance.open ( this );
        yield return new WaitForSeconds ( 0.1f );
        while ( true )
        {
            if ( Input.GetAxis ( "Fire2" ) != 0 )
            {
                PlayerController.instance.addPopulation ( cost );
                break;
            }
            if ( Input.GetAxis ( "Fire1" ) != 0 )
            {
                if ( BattleController.instance.hoverZone != null && BattleController.instance.opponentZones.Contains ( BattleController.instance.hoverZone ) )
                {
                    targetZone = BattleController.instance.hoverZone;
                    StartCoroutine ( execution ( getTargetPos () , getTargetCreatures () ) );
                }
                else
                    PlayerController.instance.addPopulation ( cost );
                break;
            }
            yield return null;
        }
        SpellCasting.instance.close ();
    }

    protected override bool costs ()
    {
        return PlayerController.instance.removePopulation ( cost );
    }

    protected override List<Creature> getTargetCreatures ()
    {
        return targetZone.creatures;
    }

    protected override Vector3 getTargetPos ()
    {
        return targetZone.transform.position;
    }

    protected override void execute ( Creature c )
    {
        c.takeDamages ( amount );
    }
}
