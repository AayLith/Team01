using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Min ( 0 )]
    public int difficulty = 10;

    [Min (0)]
    public int population = 20;
    [Min ( 0 )]
    public int populationThreshold = 5;

    [Min ( 0 )]
    public int resources = 20;
    [Min ( 0 )]
    public int resourceThreshold = 5;

    public List<Creature> units = new List<Creature> ();

    [Header ( "GameObjects" )]
    public Slider populationSlider;
    public Slider resourceSlider;

    private void Awake ()
    {
        instance = this;
    }
}
