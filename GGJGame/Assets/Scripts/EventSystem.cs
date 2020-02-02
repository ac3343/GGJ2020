using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : Singleton<EventSystem>
{
    public delegate void PowerConsumerActiveStateChangeEvent(GameObject triggered_obj, bool is_power_on);
    public static event PowerConsumerActiveStateChangeEvent PowerConsumerActiveStateChangeHandler;

    public void OnPowerConsumerActiveStateChange(GameObject triggered_obj, bool is_power_on)
    {
        PowerConsumerActiveStateChangeHandler(triggered_obj, is_power_on);
    }
    //In a real game we would probably throw an error if the game ended and we still had people listening to the event
}
