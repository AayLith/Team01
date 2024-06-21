using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overworld : MonoBehaviour
{
    public GameObject currentHoveredObject;

    [Header ( "GameObjects" )]
    public GameObject groundPlane;

    [Header ( "Others" )]
    public Vector3 mouseWorldPos;
    [SerializeField] private Vector3 posScale = new Vector3 ( 1 , 1 , 0 );

    private void Update ()
    {
        updateWorldPos ();

        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
        if ( Physics.Raycast ( ray , out raycastHit , 100f , LayerMask.GetMask ( "OverWorld" ) ) )
        {
            if ( raycastHit.transform != null )
            {
                setHoverElement ( raycastHit.transform.gameObject );
            }
        }
        else
            setHoverElement ( null );
    }

    void setHoverElement ( GameObject go )
    {
        // Remove highlight
        //currentHoveredObject.GetComponent<Outline> ().enabled = false;

        currentHoveredObject = go;
        // Add highlight
        //currentHoveredObject.GetComponent<Outline> ().enabled = true;
    }

    void updateWorldPos ()
    {
        Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
        RaycastHit hit;
        if ( groundPlane.GetComponent<Collider> ().Raycast ( ray , out hit , 10000.0F ) )
            mouseWorldPos = Vector3.Scale ( hit.point , posScale );
    }
}
