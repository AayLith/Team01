using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Min ( 0 )]
    public int difficulty = 10;

    [Min ( 0 )]
    public int population = 20;
    [Min ( 0 )]
    public int populationThreshold = 5;

    [Min ( 0 )]
    public int resources = 20;
    [Min ( 0 )]
    public int resourceThreshold = 5;

    [Min ( 0 )]
    public int power = 20;
    [Min ( 0 )]
    public int powerThreshold = 5;

    public List<Creature> units = new List<Creature> ();

    [Header ( "GameObjects" )]
    public Slider populationSlider;
    public Slider resourceSlider;
    public Slider powerSlider;

    private void Awake ()
    {
        instance = this;
    }

    private void Update ()
    {
        if ( populationSlider )
            populationSlider.value = Mathf.Lerp ( populationSlider.value , population , Time.deltaTime );
        if ( resourceSlider )
            resourceSlider.value = Mathf.Lerp ( resourceSlider.value , resources , Time.deltaTime );
        if ( powerSlider )
            powerSlider.value = Mathf.Lerp ( powerSlider.value , power , Time.deltaTime );
    }

    public void addPopulation ( int amount )
    {
        population += amount;
    }

    public bool removePopulation ( int amount )
    {
        if ( amount > population )
            return false;
        population -= amount;
        return true;
    }

    public void addResources ( int amount )
    {
        resources += amount;
    }

    public bool removeResources ( int amount )
    {
        if ( amount > resources )
            return false;
        resources -= amount;
        return true;
    }

    public void addPower ( int amount )
    {
        power += amount;
    }

    public bool removePower ( int amount )
    {
        if ( amount > power )
            return false;
        power -= amount;
        return true;
    }
}
