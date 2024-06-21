using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour, NotificationReceiver
{
    public string displayName;
    [TextArea ( 3 , 10 )]
    public string description;
    public int currentValue;
    public int startingValue;
    public int dailyChange;

    [Header ( "Options" )]
    public List<QuestOption> options = new List<QuestOption> ();

    [Header ( "GameObjects" )]
    public bool active = true;
    public GameObject UI;

    private void Awake ()
    {
        
    }

    public void clicOpen ()
    {
        if ( active == false ) return;
    }

    public void clicCloseButton ()
    {

    }

    public void clicOption (QuestOption option )
    {

    }

    public void receiveNotification ( Notification notification )
    {
        switch ( notification.name )
        {
            case Notification.notifications.startManagementPhase:
                active = true;
                break;

            case Notification.notifications.endManagementPhase:
                active = false;
                clicCloseButton ();
                break;
        }
    }
}
