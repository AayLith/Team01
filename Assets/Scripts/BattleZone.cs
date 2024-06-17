using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    public SpriteRenderer sprite;
    [Range ( 1 , 5 )]
    public int range;
    public List<Creature> creatures = new List<Creature> ();
}
