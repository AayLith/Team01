using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestOption : MonoBehaviour
{
    [TextArea ( 3 , 10 )]
    public string text;
    public int valueChange;
    public List<Ability> effects = new List<Ability> ();
}
