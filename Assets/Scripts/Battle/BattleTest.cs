using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTest : MonoBehaviour
{
    public List<Creature> playerUnits = new List<Creature> ();
    public List<Creature> opponentUnits = new List<Creature> ();

    private void Start ()
    {
        BattleController.instance.startBattlePreparation ( playerUnits , opponentUnits );
    }
}
