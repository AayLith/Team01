using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementController : PhaseMenu
{
    public static ManagementController instance;

    [Header("Management Controller")]
    public GameObject overworld;

    private void Awake ()
    {
        instance = this;
    }

    public override void open ()
    {
        overworld.SetActive ( value: true );
        PhaseTransition.instance.setText ( phaseText, helpText );
    }

    public override void close ()
    {
        overworld.SetActive ( false );
    }

    public override void end ()
    {
        PhaseTransition.instance.transition ( this , BattleController.instance );
    }
}
