using Articy.Goblinkingarticytest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BattleZone : MonoBehaviour
{
    public SpriteRenderer sprite;
    [Range ( 1 , 5 )]
    public int range;
    public List<Creature> creatures = new List<Creature> ();
    [Range ( -1 , 1 )]
    public int X_Scale;

    public void addCreature ( Creature c )
    {
        if ( creatures.Contains ( c ) )
            return;

        if ( c.zone )
            c.zone.removeCreature ( c );
        c.zone = this;
        creatures.Add ( c );
    }

    public void removeCreature ( Creature c )
    {
        if ( !creatures.Contains ( c ) )
            return;

        creatures.Remove ( c );
        c.zone = null;
    }

    public Vector3 getRandomPos ()
    {
        Vector3 pos;
        pos = new Vector3 (
            Random.Range ( sprite.bounds.min.x , sprite.bounds.max.x ) ,
            Random.Range ( sprite.bounds.min.y , sprite.bounds.max.y ) )
            - transform.position;
        return pos * 0.8f + transform.position;
    }
}
