using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfectionTimer : InfectionTimer
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //If we're a node then if we have any infection slowly increase it
        if (m_InfectionPercent > 0.0f)
        {
            IncreaseInfection(m_InfectionPercentIncreasePerSecond * Time.deltaTime);
        }
    }

    public bool IsInfected()
    {
        return m_InfectionPercent > 0.0f;
    }

    //When we receive the PowerConsumerActiveStateChange event check to see if it was us. If it was then check to see if it was to on. 
    //If it was then reset the timer and check the attached nodes to see if either of them are infected. If either of them are then infect the other

    //If we're a node then we will receive an event when we're infected with the % chance. If we aren't infected then infect and check our connections
}
