using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synergy : MonoBehaviour
{
    [TextArea ( 3 , 10 )]
    public string description;
    public List<KeyValuePair<int , List<Ability>>> abilities = new List<KeyValuePair<int, List<Ability>>>();
}
