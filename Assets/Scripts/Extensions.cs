using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public static class Extensions
{
    public static List<T> shuffle<T> ( this List<T> list )
    {
        T tmp;
        for ( int j = list.Count - 1 ; j > 0 ; j-- )
        {
            int r = Random.Range ( 0 , j );
            tmp = list[ r ];
            list[ r ] = list[ j ];
            list[ j ] = tmp;
        }
        return list;
    }

    public static T getRandomElement<T> (this List<T> list )
{
        return list[ Random.Range ( 0 , list.Count ) ];
    }

    public static T getRandomElement<T> (this T[] array )
    {
        return array[ Random.Range ( 0 , array.Length ) ];
    }
}
