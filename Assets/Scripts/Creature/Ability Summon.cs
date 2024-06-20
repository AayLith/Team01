using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySummon : Ability
{
    public Creature summon;
    [Range ( 0 , 10 )]
    public int amount;
    public enum modes { self_zone, random_zone, zone1, zone2, zone3 }
    public modes mode;
    BattleZone bzTarget;

    public override void setTargets ( List<BattleZone> allyZones , List<BattleZone> ennemyZones , Creature _caster = null )
    {
        targets.Clear ();
        targets.Add ( caster );

        switch ( mode )
        {
            case modes.self_zone:
                bzTarget = caster.zone;
                break;
            case modes.random_zone:
                bzTarget = allyZones.getRandomElement ();
                break;
            case modes.zone1:
                bzTarget = allyZones[ 0 ];
                foreach ( BattleZone bz in allyZones )
                    if ( bz.range == 1 )
                        bzTarget = bz;
                break;
            case modes.zone2:
                foreach ( BattleZone bz in allyZones )
                    if ( bz.range == 2 )
                        bzTarget = bz;
                break;
            case modes.zone3:
                // Get the right one or the farthest one
                foreach ( BattleZone bz in allyZones )
                    if ( bz.range == 3 )
                        bzTarget = bz;
                    else if ( bz.range > bzTarget.range )
                        bzTarget = bz;
                break;
        }
    }

    protected override IEnumerator animationSecondPart ()
    {
        // For each summon + Projectile
        for ( int i = 0 ; i < amount ; i++ )
        {
            Vector3 pos = bzTarget.getRandomPos ();

            yield return StartCoroutine ( attackWindown ( pos ) );

            // Invocation
            Creature c = Instantiate ( summon.gameObject , pos , summon.transform.rotation ).GetComponent<Creature> ();
            bzTarget.addCreature ( c );

            yield return new WaitForSeconds ( 0.1f );
        }
        yield return new WaitForSeconds ( 0.2f );
    }
}
