using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementController : PhaseMenu
{
    public static ManagementController instance;

    [Header ( "Management Controller" )]
    public GameObject overworld;
    public GameObject canvas;

    MapLocation currentLocation;
    Quest currentQuest;

    private void Awake ()
    {
        instance = this;
    }

    private void Start ()
    {
        open ();
    }

    private void Update ()
    {
        if ( currentQuest )
            return;

        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
        if ( Physics.Raycast ( ray , out raycastHit , 100f , LayerMask.GetMask ( "Overworld" ) ) )
        {
            if ( raycastHit.transform != null )
            {
                setHoverLocation ( raycastHit.transform.gameObject.GetComponent<MapLocation> () );
            }
        }
        else setHoverLocation ( null );

        if ( Input.GetAxis ( "Fire1" ) != 0 )
        {
            // TODO Open quest and set as currentQuest
        }
    }

    void setHoverLocation ( MapLocation loc )
    {
        if ( currentLocation != null && loc == currentLocation ) return;

        if ( currentLocation != null )
            currentLocation.onPointerExit ();

        if ( loc == null )
        {
            currentLocation = null;
        }
        else
        {
            currentLocation = loc;
            currentLocation.onPointerEnter ();
        }
    }

    public override void open ()
    {
        overworld.SetActive ( value: true );
        canvas.SetActive ( true );
        PhaseTransition.instance.setText ( phaseText , helpText );
    }

    public override void close ()
    {
        overworld.SetActive ( false );
        canvas.SetActive ( false );
    }

    public override void end ()
    {
        // TODO close active quest
        canvas.SetActive ( false );
        PhaseTransition.instance.transition ( this , BattleController.instance );
    }
}
