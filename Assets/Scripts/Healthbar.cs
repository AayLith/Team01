using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public int verticalOffset;
    public Slider slider;
    RectTransform rect;

    public void init ( int val , int max , int width , int height , int offset )
    {
        rect = slider.GetComponent<RectTransform> ();
        verticalOffset = offset;
        slider.minValue = 0;
        slider.maxValue = max;
        slider.value = val;
        rect.sizeDelta = new Vector2 ( width , height );
    }

    public void updatePos ( Creature c )
    {
        rect.position = Camera.main.WorldToScreenPoint ( c.transform.position ) + new Vector3 ( 0 , verticalOffset , 0 );
    }

    public void updateValue ( int val )
    {
        slider.value = val;
    }
}
