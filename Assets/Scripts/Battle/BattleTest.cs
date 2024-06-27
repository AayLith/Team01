using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTest : PhaseMenu
{
    public int budget;
    public List<Creature> playerUnits = new List<Creature> ();
    public List<Creature> opponentUnits = new List<Creature> ();

    private void Start ()
    {
        PhaseTransition.instance.transition ( this , BattleController.instance );
        BattleController.instance.startBattlePreparation ( playerUnits , ArmyBuilder.buildArmy ( opponentUnits , budget , false ) );
    }

    public override void open ()
    {

    }

    public override void close ()
    {

    }

    public override void end ()
    {

    }
}
