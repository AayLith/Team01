using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TMPro;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    [Header ( "GameObjects" )]
    public GameObject groundPlane;
    public GameObject startButton;
    public TMP_Text battleText;

    [Header ( "Zones" )]
    public List<Creature> playerUnits = new List<Creature> ();
    public List<Creature> opponentUnits = new List<Creature> ();

    public List<BattleZone> playerZones = new List<BattleZone> ();
    public List<BattleZone> opponentZones = new List<BattleZone> ();
    public BattleZone playerReserve;

    public enum battlePhases { preparation, battle, ending }
    public battlePhases currentPhase;

    [Header ( "Mouse hover" )]
    public Creature hoverUnit = null;
    public BattleZone hoverZone = null;
    [SerializeField] private Vector3 posScale = new Vector3 ( 1 , 1 , 0 );
    public Vector3 unitSelectOffset = new Vector3 ( 0 , 0 , 0.25f );
    public Vector3 mouseWorldPos;



    // Private stuff
    int movingUnits = 0;


    private void Awake ()
    {
        instance = this;
    }

    private void Update ()
    {
        updateWorldPos ();

        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
        if ( Physics.Raycast ( ray , out raycastHit , 100f , LayerMask.GetMask ( "PlayerUnit" ) ) )
        {
            if ( raycastHit.transform != null )
            {
                setHoverUnit ( raycastHit.transform.gameObject.GetComponent<Creature> () );
            }
        }
        else
            setHoverUnit ( null );

        if ( Physics.Raycast ( ray , out raycastHit , 100f , LayerMask.GetMask ( "BattleZone" ) ) )
        {
            if ( raycastHit.transform != null )
            {
                setHoverZone ( raycastHit.transform.gameObject.GetComponent<BattleZone> () );
            }
        }
        else
            setHoverZone ( null );
    }

    public void startBattlePreparation ( List<Creature> player , List<Creature> opponent )
    {
        playerUnits.Clear ();
        opponentUnits = opponent;
        currentPhase = battlePhases.preparation;

        foreach ( Creature c in opponentUnits )
        {
            // TODO Instanciate creatures

            // Place creatures inside their prefered zones
            // Chose the best zone
            BattleZone zone = opponentZones[ 0 ];
            foreach ( BattleZone bz in opponentZones )
                if ( bz.range == c.preferedRange )
                {
                    zone = bz;
                    break;
                }
                else if ( bz.range > zone.range && bz.range <= c.preferedRange )
                    zone = bz;
            c.spriteRenderer.transform.localScale = Vector3.Scale ( c.spriteRenderer.transform.localScale , new Vector3 ( zone.X_Scale , 1 , 1 ) );
            c.spriteRenderer.transform.localScale = new Vector3 ( -1 , 1 , 1 );
            moveUnitToZone ( zone , c );
        }

        foreach ( Creature c in player )
        {
            // TODO Instanciate creatures

            // Placer creatures inside player reserve zone
            moveUnitToZone ( playerReserve , c );
        }


        StartCoroutine ( battlePreparation () );
    }

    IEnumerator battlePreparation ()
    {
        Creature currentUnit = null;
        battleText.text = "Preparation";
        while ( currentPhase == battlePhases.preparation )
        {
            currentUnit = hoverUnit;

            // If L-click on a player creature, start dragging
            if ( Input.GetAxis ( "Fire1" ) != 0 && currentUnit != null )
            {
                currentUnit.zone = null;
                while ( Input.GetAxis ( "Fire1" ) != 0 )
                {
                    currentUnit.transform.position = mouseWorldPos + unitSelectOffset;
                    currentUnit.spriteRenderer.transform.rotation = Quaternion.Euler ( new Vector3 (
                        currentUnit.spriteRenderer.transform.rotation.eulerAngles.x ,
                        currentUnit.spriteRenderer.transform.rotation.eulerAngles.y ,
                        Mathf.Sin ( Time.time * 20 ) * 25 ) );
                    yield return null;
                }
                // If cursor is over a player zone
                if ( playerZones.Contains ( hoverZone ) || hoverZone == playerReserve )
                    moveUnitToZone ( hoverZone , currentUnit , mouseWorldPos );
                else
                    moveUnitToZone ( playerReserve , currentUnit );
            }

            // If R-click on a player creature, return it to the reserve
            else if ( Input.GetAxis ( "Fire2" ) != 0 && currentUnit != null )
                moveUnitToZone ( hoverZone , currentUnit );

            yield return null;
        }
    }

    public void startBattleButton ()
    {
        startButton.SetActive ( false );
        currentPhase = battlePhases.battle;
        StartCoroutine ( battleActing () );
    }

    void gatherOpponentUnits ()
    {
        opponentUnits = new List<Creature> ();
        foreach ( BattleZone bz in opponentZones )
            playerUnits.AddRange ( bz.creatures );
    }

    IEnumerator battleActing ()
    {
        playerUnits = new List<Creature> ();
        foreach ( BattleZone bz in playerZones )
            playerUnits.AddRange ( bz.creatures );
        gatherOpponentUnits ();

        //////////////////////////////////////////////////////////////////////////////
        // Before battle
        // Some abilities and things happens here
        battleText.text = "Battle Start";
        yield return StartCoroutine ( executeAbilities ( Ability.triggers.battleStart ) );

        //////////////////////////////////////////////////////////////////////////////
        // During battle
        for ( int i = 0 ; i < 3 ; i++ )
        {
            // Start of turn
            // Some abilities and things happens here
            battleText.text = "Turn " + ( i + 1 ) + " Start";
            yield return StartCoroutine ( executeAbilities ( Ability.triggers.turnStart ) );


            // Turn execution
            // Make units attack and cast their abilities
            gatherOpponentUnits ();
            battleText.text = "Turn " + ( i + 1 ) + " Execution";
            yield return StartCoroutine ( executeAbilities ( Ability.triggers.turn ) );
            yield return StartCoroutine ( executeAttacks () );

            List<Creature> creatures = new List<Creature> ();
            creatures.AddRange ( playerUnits );
            creatures.AddRange ( opponentUnits );
            // Shuffle the list
            creatures = creatures.shuffle ();
            // Set targets
            /*
            foreach ( BattleZone plz in playerZones )
                foreach ( Creature attacker in plz.creatures )
                {
                    int range = 0;
                    List<Creature> targets = new List<Creature> ();
                    // Gather all units in range
                    foreach ( BattleZone opz in opponentZones )
                        if ( plz.range + opz.range - 1 <= attacker.atkRange )
                        {
                            targets.AddRange ( opz.creatures );
                            range = plz.range + opz.range - 1;
                        }
                    // Choose a target at random
                    if ( targets.Count > 0 )
                    {
                        attacker.target = targets[ Random.Range ( 0 , targets.Count ) ];
                        attacker.targetRange = range;
                    }
                }
            foreach ( BattleZone plz in opponentZones )
                foreach ( Creature attacker in plz.creatures )
                {
                    int range = 0;
                    List<Creature> targets = new List<Creature> ();
                    // Gather all units in range
                    foreach ( BattleZone opz in playerZones )
                        if ( plz.range + opz.range - 1 <= attacker.atkRange )
                        {
                            targets.AddRange ( opz.creatures );
                            range = plz.range + opz.range - 1;
                        }
                    // Choose a target at random
                    if ( targets.Count > 0 )
                    {
                        attacker.target = targets[ Random.Range ( 0 , targets.Count ) ];
                        attacker.targetRange = range;
                    }
                }
            */
            // Attacks and Abilities
            /*
            foreach ( Creature attacker in creatures )
                if ( attacker.target != null )
                    yield return StartCoroutine ( attack ( attacker , attacker.target ) );
            */
            // Remove targets
            /*
            foreach ( Creature attacker in creatures )
                attacker.target = null;
            */

            // End of turn
            // Some abilities and things happens here
            // Kill units with 0 hp and clamp unit hp
            battleText.text = "Turn " + ( i + 1 ) + " End";
            foreach ( Creature c in creatures )
                if ( c.curhealth > c.health ) c.curhealth = c.health;
                else if ( c.curhealth <= 0 && !c.isdying )
                {
                    StartCoroutine ( killUnit ( c ) );
                    yield return new WaitForSeconds ( 0.05f );
                }

            gatherOpponentUnits ();
            yield return StartCoroutine ( executeAbilities ( Ability.triggers.turnEnd ) );

            // Move units forward if zone 1 is empty
            BattleZone pz = getZone ( 'p' , 1 );
            BattleZone oz = getZone ( 'o' , 1 );
            if ( pz.creatures.Count == 0 )
                for ( int j = 1 ; j < playerZones.Count ; j++ )
                    foreach ( Creature c in playerZones[ j ].creatures )
                        StartCoroutine ( walkUnit ( c , c.transform.position + new Vector3 ( 0.3f , 0 , 0 ) ) );
            if ( oz.creatures.Count == 0 )
                for ( int j = 1 ; j < opponentUnits.Count ; j++ )
                    foreach ( Creature c in opponentZones[ j ].creatures )
                        StartCoroutine ( walkUnit ( c , c.transform.position + new Vector3 ( -0.3f , 0 , 0 ) ) );
            while ( movingUnits > 0 )
                yield return null;

            yield return null;
            yield return new WaitForSeconds ( 1f );
        }

        //////////////////////////////////////////////////////////////////////////////
        // After battle
        // Some abilities and things happens here
        yield return StartCoroutine ( executeAbilities ( Ability.triggers.battleEnd ) );
    }

    IEnumerator executeAbilities ( Ability.triggers trigger )
    {
        List<Ability> allAbilities = new List<Ability> ();

        foreach ( Creature c in playerUnits )
            foreach ( Ability a in c.abilities )
                if ( a.trigger == trigger )
                {
                    allAbilities.Add ( a );
                    a.setTargets ( playerZones , opponentZones );
                }
        foreach ( Creature c in opponentUnits )
            foreach ( Ability a in c.abilities )
                if ( a.trigger == trigger )
                {
                    allAbilities.Add ( a );
                    a.setTargets ( playerZones , opponentZones );
                }

        allAbilities.shuffle ();
        foreach ( Ability a in allAbilities )
            yield return StartCoroutine ( a.execute () );
    }

    IEnumerator executeAttacks ()
    {
        List<Ability> allAbilities = new List<Ability> ();

        foreach ( Creature c in playerUnits )
        {
            if ( c.attack == null )
                continue;
            allAbilities.Add ( c.attack );
            c.attack.setTargets ( playerZones , opponentZones );
        }
        foreach ( Creature c in opponentUnits )
        {
            if ( c.attack == null )
                continue;
            allAbilities.Add ( c.attack );
            c.attack.setTargets ( opponentZones , playerZones );
        }

        allAbilities.shuffle ();
        foreach ( Ability a in allAbilities )
            yield return StartCoroutine ( a.execute () );
    }

    IEnumerator walkUnit ( Creature c , Vector3 pos )
    {
        movingUnits++;
        float elapsedTime = 0f;
        // Walk animation
        while ( c.transform.position != pos )
        {
            c.transform.position = Vector3.MoveTowards ( c.transform.position , pos , 0.1f );
            c.spriteRenderer.transform.localPosition = new Vector3 ( 0 , 1 , 0 ) * Mathf.Abs ( Mathf.Sin ( elapsedTime * Mathf.PI ) );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        movingUnits--;
    }

    BattleZone getZone ( char side , int range )
    {
        if ( side == 'p' )
        {
            foreach ( BattleZone bz in playerZones )
                if ( bz.range == range )
                    return bz;
        }
        else
        {
            foreach ( BattleZone bz in opponentZones )
                if ( bz.range == range )
                    return bz;
        }
        return null;
    }

    IEnumerator killUnit ( Creature c )
    {
        c.isdying = true;
        c.zone.removeCreature ( c );
        try { playerUnits.Remove ( c ); }
        catch { }
        try { opponentUnits.Remove ( c ); }
        catch { }

        // Death animation
        for ( int i = 1 ; i < 7 ; i++ )
        {
            c.spriteRenderer.transform.rotation = Quaternion.Euler ( new Vector3 (
                c.spriteRenderer.transform.rotation.eulerAngles.x ,
                c.spriteRenderer.transform.rotation.eulerAngles.y ,
                 i * -5 ) );
            yield return null;
        }

        Destroy ( c.gameObject );
    }

    private void updateWorldPos ()
    {
        Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
        RaycastHit hit;
        if ( groundPlane.GetComponent<Collider> ().Raycast ( ray , out hit , 10000.0F ) )
            mouseWorldPos = Vector3.Scale ( hit.point , posScale );
    }

    void setHoverUnit ( Creature c )
    {
        if ( hoverUnit != null && c == hoverUnit ) return;

        if ( c == null )
        {
            hoverUnit = null;
        }
        else
        {
            hoverUnit = c;
        }
    }

    void setHoverZone ( BattleZone bz )
    {
        if ( hoverZone != null && bz == hoverZone ) return;

        if ( bz == null )
        {
            if ( hoverZone != null )
            {
                setZoneColor ( hoverZone , 0.25f );
                hoverZone = null;
            }
        }
        else
        {
            if ( hoverZone != null )
                setZoneColor ( hoverZone , 0.25f );
            hoverZone = bz;
            setZoneColor ( hoverZone , 1f );
        }
    }

    void setZoneColor ( BattleZone bz , float a )
    {
        Color c;
        c = hoverZone.sprite.color;
        c.a = a;
        hoverZone.sprite.color = c;
    }

    void moveUnitToZone ( BattleZone zone , Creature unit , Vector3 pos = new Vector3 () )
    {
        if ( pos == Vector3.zero )
            unit.transform.position = zone.getRandomPos ();
        else
            unit.transform.position = pos;

        unit.spriteRenderer.transform.localScale = Vector3.Scale ( new Vector3 ( Mathf.Sign ( unit.spriteRenderer.transform.localScale.x ) * unit.spriteRenderer.transform.localScale.x , unit.spriteRenderer.transform.localScale.y , unit.spriteRenderer.transform.localScale.z ) , new Vector3 ( zone.X_Scale , 1 , 1 ) );
        unit.spriteRenderer.transform.rotation = Quaternion.Euler ( new Vector3 ( -90 , 0 , 0 ) );
        zone.addCreature ( unit );
    }
}