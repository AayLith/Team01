using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArmyBuilder
{
    public static Dictionary<Creature , int> buildArmy ( List<Creature> availableUnits , int budget , bool enforceBoss )
    {
        Dictionary<Creature , int> army = new Dictionary<Creature , int> ();
        List<Creature> bosses = new List<Creature> ();
        Creature current;

        // Extract bosses
        foreach ( Creature c in availableUnits )
            if ( c.boss && c.cost < budget )
                bosses.Add ( c );
        foreach ( Creature c in bosses )
            availableUnits.Remove ( c );

        // Add a boss if we want one
        if ( enforceBoss )
        {
            if ( bosses.Count > 0 )
            {
                current = bosses.getRandomElement ();
                army.Add ( current , 1 );
                budget -= current.cost;
            }
        }

        // Make units popular !
        for ( int i = availableUnits.Count - 1 ; i != -1 ; i-- )
            if ( availableUnits[ i ].popularity > 0 )
                for ( int j = 0 ; j < availableUnits[ i ].popularity ; j++ )
                    availableUnits.Add ( availableUnits[ i ] );

        // Add regular units
        while ( availableUnits.Count > 0 )
        {
            // Get a random unit and add to the army
            current = availableUnits.getRandomElement ();
            if ( !addUnitToArmy ( current , budget ) )
                break;
            else
            {
                if ( army.ContainsKey ( current ) )
                    army[ current ]++;
                else
                    army.Add ( current , 1 );
                budget -= ( current.cost + army[ current ] * current.incrementalCost );
            }
            // Remove units that cost too much
            for ( int i = availableUnits.Count - 1 ; i != -1 ; i-- )
                if ( availableUnits[ i ].cost > budget )
                    availableUnits.RemoveAt ( i );

            if ( army.Count > 1000 )
                break;
        }

        return army;
    }

    // Add a unit to the army if we have enough budget
    static bool addUnitToArmy ( Creature c , int budget )
    {
        if ( c.cost == 0 )
        {
            Debug.LogError ( "Error : creature " + c.name + " has a cost of 0, army building has been canceled" );
            return false;
        }
        // Only the first unit may be too pricey
        if ( c.cost > budget )
            return true;

        return true;
    }
}
