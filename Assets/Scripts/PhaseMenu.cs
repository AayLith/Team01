using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhaseMenu : MonoBehaviour
{
    [Header ( "Phase Menu" )]
    public string phaseText;
    [TextArea ( 3 , 10 )]
    public string helpText;

    public abstract void open ();

    public abstract void close ();

    public abstract void end ();
}
