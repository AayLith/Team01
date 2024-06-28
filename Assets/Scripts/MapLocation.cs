using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapLocation : MonoBehaviour
{
    public Transform mapName;
    public GameObject background;
    float scale;

    private void Start ()
    {
        background.SetActive ( false );
        scale = mapName.localScale.x;
    }

    public void onPointerEnter ()
    {
        background.SetActive ( true );
        mapName.localScale = Vector3.one * scale * 1.1f;
    }

    public void onPointerExit ()
    {
        background.SetActive ( false );
        mapName.localScale = Vector3.one * scale;
    }
}
