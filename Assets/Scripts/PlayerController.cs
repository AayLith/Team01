using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int difficulty;

    public int population = 20;
    public int populationThreshold = 5;

    public int resources = 20;
    public int resourceThreshold = 5;

    public List<Creature> units = new List<Creature> ();

    [Header ( "GameObjects" )]
    public Slider populationSlider;
    public Slider resourceSlider;
}
