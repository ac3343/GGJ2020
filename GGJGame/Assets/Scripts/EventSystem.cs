using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : Singleton<EventSystem>
{
    public delegate void PowerConsumerActiveStateChangeEvent();
    public static event PowerConsumerActiveStateChangeEvent PowerConsumerActiveStateChangeHandler;

    public void OnPowerConsumerActiveStateChange()
    {
        PowerConsumerActiveStateChangeHandler();
    }
    //In a real game we would probably throw an error if the game ended and we still had people listening to the event
}
