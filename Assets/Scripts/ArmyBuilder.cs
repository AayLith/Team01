using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyBuilder
{
    public List<Creature> buildArmy ( List<Creature> availableUnits , int budget , bool enforceBoss )
    {
        List<Creature> army = new List<Creature> ();
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
                army.Add ( bosses.getRandomElement () );
                budget -= army[ 0 ].cost;
            }
        }

        // Add regular units
        while ( availableUnits.Count > 0 )
        {
            // Get a random unit and add to the army
            current = availableUnits.getRandomElement ();
            if ( !addUnitToArmy ( current , army , budget ) )
                break;
            budget -= current.cost;

            // Remove units that cost too much
            for ( int i = availableUnits.Count - 1 ; i != 0 ; i-- )
                if ( availableUnits[ i ].cost > budget )
                    availableUnits.RemoveAt ( i );
        }

        return army;
    }

    // Add a unit to the army if we have enough budget
    bool addUnitToArmy ( Creature c , List<Creature> army , int budget )
    {
        if ( c.cost == 0 )
        {
            Debug.LogError ( "Error : creature " + c.name + " has a cost of 0, army building has been canceled" );
            return false;
        }
        // Only the first unit may be too pricey
        if ( c.cost > budget )
            return true;

        army.Add ( c );
        return true;
    }
}
