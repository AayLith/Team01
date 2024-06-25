using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TMPro;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    [Header ( "GameObjects" )]
    public GameObject battlefieldObj;
    public GameObject playerUnitsObj;
    public GameObject opponentUnitsObj;
    public GameObject groundPlane;
    public GameObject startButton;
    public TMP_Text battleText;

    [Header ( "UI" )]
    public GameObject battleReport;
    public ReportLine reportLine;
    public Transform linesLayout;
    public CanvasGroup bonusGroup;
    public CanvasGroup lootGroup;
    public CanvasGroup popGroup;
    public TMP_Text battleOutcome;
    public TMP_Text bonusText;
    public TMP_Text lootText;
    public TMP_Text populationText;

    [Header ( "Zones" )]
    public List<Creature> playerUnits = new List<Creature> ();
    public List<Creature> opponentUnits = new List<Creature> ();
    Dictionary<Creature , int> deadAllies = new Dictionary<Creature , int> ();
    Dictionary<Creature , int> deadEnnemies = new Dictionary<Creature , int> ();

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
    int lootBonus = 0;

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
        else if ( Physics.Raycast ( ray , out raycastHit , 100f , LayerMask.GetMask ( "OpponentUnit" ) ) )
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

        if ( Input.GetAxis ( "Fire1" ) != 0 && hoverUnit != null )
            UnitTooltip.instance.setToolTip ( hoverUnit );
        else
            UnitTooltip.instance.hide ();
    }

    public void startBattlePreparation ( List<Creature> player , List<Creature> opponent )
    {
        Dictionary<Creature , int> army = new Dictionary<Creature , int> ();
        foreach ( Creature c in opponent )
            if ( army.ContainsKey ( c ) )
                army[ c ]++;
            else
                army.Add ( c , 1 );
        startBattlePreparation ( player , army );
    }

    public void startBattlePreparation ( List<Creature> player , Dictionary<Creature , int> opponent )
    {
        // Clear some stuff
        lootBonus = 0;
        playerUnits.Clear ();
        opponentUnits.Clear ();
        deadAllies.Clear ();
        deadEnnemies.Clear ();
        currentPhase = battlePhases.preparation;

        foreach ( KeyValuePair<Creature , int> kvp in opponent )
        {
            Creature c;
            // TODO Instanciate creatures
            for ( int i = 0 ; i < kvp.Value ; i++ )
            {
                c = Instantiate ( kvp.Key.gameObject , opponentUnitsObj.transform ).GetComponent<Creature> ();
                // Place creatures inside their prefered zones
                moveUnitToZone ( getPreferedZone ( c ) , c );
                opponentUnits.Add ( c );
                if ( opponentUnits.Count > 1000 )
                    break;
            }
        }

        foreach ( Creature c in player )
        {
            // TODO Instanciate creatures

            // Placer creatures inside player reserve zone
            moveUnitToZone ( playerReserve , c );
        }

        battlefieldObj.SetActive ( true );
        playerUnitsObj.SetActive ( true );
        opponentUnitsObj.SetActive ( true );
        StartCoroutine ( battlePreparation () );
    }

    IEnumerator battlePreparation ()
    {
        Creature currentUnit = null;
        battleText.text = "Preparation";
        while ( currentPhase == battlePhases.preparation )
        {
            currentUnit = hoverUnit;

            if ( Input.GetAxis ( "Fire1" ) != 0 && currentUnit != null )
            {
                // If L-click on a player creature, start dragging
                if ( currentUnit.tag == "PlayerUnit" )
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
                    currentUnit.spriteRenderer.transform.rotation = Quaternion.Euler ( new Vector3 ( currentUnit.spriteRenderer.transform.rotation.eulerAngles.x , 0 , 0 ) );

                }
                else /*if (currentUnit.tag == "OpponentUnit" )*/
                {
                    while ( Input.GetAxis ( "Fire1" ) != 0 )
                        yield return null;
                }
            }

            // If R-click on a player creature, return it to the reserve, or place it into its preferedZone if it is already in the reserve
            else if ( Input.GetAxis ( "Fire2" ) != 0 && currentUnit != null )
                if ( currentUnit.zone != playerReserve )
                    moveUnitToZone ( playerReserve , currentUnit );
                else
                {
                    BattleZone bz = getZone ( 'p' , currentUnit.preferedRange );
                    if ( bz ) moveUnitToZone ( bz , currentUnit );
                }

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
            opponentUnits.AddRange ( bz.creatures );
    }

    void gatherPlayerUnits ()
    {
        playerUnits = new List<Creature> ();
        foreach ( BattleZone bz in playerZones )
            playerUnits.AddRange ( bz.creatures );
    }

    IEnumerator battleActing ()
    {
        gatherPlayerUnits ();
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
            gatherPlayerUnits ();
            gatherOpponentUnits ();

            // Start of turn
            // Some abilities and things happens here
            battleText.text = "Turn " + ( i + 1 ) + " Start";
            yield return StartCoroutine ( executeAbilities ( Ability.triggers.turnStart ) );


            // Turn execution
            // Make units attack and cast their abilities
            battleText.text = "Turn " + ( i + 1 ) + " Execution";
            yield return StartCoroutine ( executeAbilities ( Ability.triggers.turn ) );
            yield return StartCoroutine ( executeAttacks () );

            List<Creature> creatures = new List<Creature> ();
            creatures.AddRange ( playerUnits );
            creatures.AddRange ( opponentUnits );
            // Shuffle the list
            creatures = creatures.shuffle ();

            // End of turn
            // Some abilities and things happens here
            // Kill units with 0 hp and clamp unit hp
            battleText.text = "Turn " + ( i + 1 ) + " End";
            foreach ( Creature c in playerUnits )
                if ( c.curhealth > c.health ) c.curhealth = c.health;
                else if ( c.curhealth <= 0 && !c.isDead )
                {
                    StartCoroutine ( killUnit ( c , deadAllies ) );
                    yield return new WaitForSeconds ( 0.05f );
                }
            foreach ( Creature c in opponentUnits )
                if ( c.curhealth > c.health ) c.curhealth = c.health;
                else if ( c.curhealth <= 0 && !c.isDead )
                {
                    StartCoroutine ( killUnit ( c , deadEnnemies ) );
                    yield return new WaitForSeconds ( 0.05f );
                }

            yield return StartCoroutine ( executeAbilities ( Ability.triggers.turnEnd ) );

            // Move units forward if zone 1 is empty
            gatherPlayerUnits ();
            gatherOpponentUnits ();
            BattleZone pz = getZone ( 'p' , 1 );
            BattleZone oz = getZone ( 'o' , 1 );
            if ( pz.creatures.Count == 0 )
                for ( int j = 1 ; j < playerZones.Count ; j++ )
                    foreach ( Creature c in playerZones[ j ].creatures )
                        StartCoroutine ( walkUnit ( c , c.transform.position + new Vector3 ( 0.55f , 0 , 0 ) , pz ) );
            if ( oz.creatures.Count == 0 )
                for ( int j = 1 ; j < opponentUnits.Count ; j++ )
                    foreach ( Creature c in opponentZones[ j ].creatures )
                        StartCoroutine ( walkUnit ( c , c.transform.position + new Vector3 ( -0.55f , 0 , 0 ) , oz ) );
            while ( movingUnits > 0 )
                yield return null;

            yield return null;
            yield return new WaitForSeconds ( 1f );
        }

        //////////////////////////////////////////////////////////////////////////////
        // After battle
        // Some abilities and things happens here
        yield return StartCoroutine ( executeAbilities ( Ability.triggers.battleEnd ) );

        StartCoroutine ( endBattle () );
    }

    IEnumerator executeAbilities ( Ability.triggers trigger )
    {
        List<Ability> allAbilities = new List<Ability> ();

        foreach ( Creature c in playerUnits )
            if ( c.isDead ) continue;
            else
                foreach ( Ability a in c.abilities )
                    if ( a.trigger == trigger )
                    {
                        allAbilities.Add ( a );
                        a.setTargets ( playerZones , opponentZones );
                    }
        foreach ( Creature c in opponentUnits )
            if ( c.isDead ) continue;
            else
                foreach ( Ability a in c.abilities )
                    if ( a.trigger == trigger )
                    {
                        allAbilities.Add ( a );
                        a.setTargets ( opponentZones , playerZones );
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
            if ( c.isDead || c.attack == null )
                continue;
            allAbilities.Add ( c.attack );
            c.attack.setTargets ( playerZones , opponentZones );
        }
        foreach ( Creature c in opponentUnits )
        {
            if ( c.isDead || c.attack == null )
                continue;
            allAbilities.Add ( c.attack );
            c.attack.setTargets ( opponentZones , playerZones );
        }

        allAbilities.shuffle ();
        foreach ( Ability a in allAbilities )
            yield return StartCoroutine ( a.execute () );
    }

    IEnumerator walkUnit ( Creature c , Vector3 pos , BattleZone z )
    {
        movingUnits++;
        float elapsedTime = 0f;
        // Walk animation
        while ( c.transform.position != pos )
        {
            c.transform.position = Vector3.MoveTowards ( c.transform.position , pos , 0.025f );
            c.spriteRenderer.transform.rotation = Quaternion.Euler ( new Vector3 (
                c.spriteRenderer.transform.rotation.eulerAngles.x ,
                c.spriteRenderer.transform.rotation.eulerAngles.y ,
                Mathf.Sin ( elapsedTime * 20 ) * 25 ) );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        c.spriteRenderer.transform.rotation = Quaternion.Euler ( new Vector3 ( c.spriteRenderer.transform.rotation.eulerAngles.x , 0 , 0 ) );
        movingUnits--;
        moveUnitToZone ( z , c , pos );
    }

    IEnumerator killUnit ( Creature c , Dictionary<Creature , int> deads )
    {
        c.isDead = true;
        bool found = false;

        foreach ( KeyValuePair<Creature , int> kvp in deads )
            if ( kvp.Key.displayName == c.displayName )
            {
                deads[ kvp.Key ]++;
                found = true;
                break;
            }
        if ( !found )
            deads.Add ( c , 1 );

        c.zone.removeCreature ( c );

        // Death animation
        for ( int i = 1 ; i < 7 ; i++ )
        {
            c.spriteRenderer.transform.rotation = Quaternion.Euler ( new Vector3 (
                c.spriteRenderer.transform.rotation.eulerAngles.x ,
                c.spriteRenderer.transform.rotation.eulerAngles.y ,
                 i * -5 ) );
            yield return null;
        }

        Color color = c.spriteRenderer.color;
        for ( int i = 1 ; i < 11 ; i++ )
        {
            color.a -= 0.1f;
            c.spriteRenderer.color = color;
            c.healthbar.slider.GetComponent<CanvasGroup> ().alpha = color.a;
            yield return null;
        }
    }

    IEnumerator endBattle ()
    {
        gatherPlayerUnits ();
        gatherOpponentUnits ();
        int loot = 0;

        battleOutcome.text = playerUnits.Count <= 0 ? "Defeat" : "Victory";

        // Hide battle
        battlefieldObj.SetActive ( false );
        playerUnitsObj.SetActive ( false );
        opponentUnitsObj.SetActive ( false );

        // Open and fill in battle results
        // Dead creatures on each sides
        foreach ( KeyValuePair<Creature , int> c in deadAllies )
            ;
        foreach ( KeyValuePair<Creature , int> c in deadEnnemies )
        {
            ReportLine rl = Instantiate ( reportLine , linesLayout ).GetComponent<ReportLine> ();
            rl.sprite.sprite = c.Key.spriteRenderer.sprite;
            rl.creature.text = c.Key.displayName;
            rl.killed.text = "" + c.Value;
            loot += c.Value * c.Key.loot;
            rl.loot.text = "" + loot;
        }

        // Battle Report
        bonusGroup.alpha = 0;
        lootGroup.alpha = 0;
        popGroup.alpha = 0;
        battleReport.SetActive ( true );
        StartCoroutine ( canvasGroupFade ( bonusGroup , 0 , 1 , 1 ) );
        StartCoroutine ( battleLoot ( lootBonus , bonusText ) );
        yield return new WaitForSeconds ( 0.5f );
        StartCoroutine ( canvasGroupFade ( lootGroup , 0 , 1 , 1 ) );
        StartCoroutine ( battleLoot ( loot + lootBonus , lootText ) );
        yield return new WaitForSeconds ( 0.5f );
        StartCoroutine ( canvasGroupFade ( popGroup , 0 , 1 , 1 ) );
        StartCoroutine ( battleLoot ( playerUnits.Count <= 0 ? -5 : +5 , populationText ) );

        PlayerController.instance.addResources ( loot + lootBonus );
        PlayerController.instance.addPopulation ( playerUnits.Count <= 0 ? -5 : +5 );

        // Destroy dead units and move player units to reserve
        foreach ( Creature c in opponentUnits )
            if ( c.isDead )
                Destroy ( c.gameObject );
        foreach ( Creature c in playerUnits )
            if ( c.isDead )
                Destroy ( c.gameObject );
            else
                moveUnitToZone ( playerReserve , c );
    }

    IEnumerator battleLoot ( int val , TMP_Text target )
    {
        target.text = "0";
        float length = 3;
        float step = val / length;
        float value = 0;

        while ( length > 0 )
        {
            value += step;
            target.text = "" + Mathf.FloorToInt ( value );
            length -= Time.fixedDeltaTime;
            yield return null;
        }
        target.text = "" + val;
    }

    IEnumerator canvasGroupFade ( CanvasGroup group , float start , float end , float length )
    {
        float step = length / ( end - start );
        while ( length > 0 )
        {
            group.alpha -= step;
            length -= Time.fixedDeltaTime;
            yield return null;
        }
        group.alpha = end;
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

        //if ( hoverUnit )
        //  hoverUnit.outline.enabled = false;

        if ( c == null )
        {
            hoverUnit = null;
        }
        else
        {
            hoverUnit = c;
            //hoverUnit.outline.enabled = true;
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
        zone.addCreature ( unit );
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

    BattleZone getPreferedZone ( Creature c )
    {
        BattleZone zone = opponentZones[ 0 ];
        foreach ( BattleZone bz in opponentZones )
            if ( bz.range == c.preferedRange )
            {
                zone = bz;
                break;
            }
            else if ( bz.range > zone.range && bz.range <= c.preferedRange )
                zone = bz;
        return zone;
    }

    // True if the unit should belong to the player, false if it belongs to the opponent
    public void addUnit ( Creature c , bool playerSide )
    {
        if ( playerSide )
            playerReserve.addCreature ( c );
        else
            moveUnitToZone ( getPreferedZone ( c ) , c );
    }
}
