using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionInfectionTimer : InfectionTimer
{
    [SerializeField]
    float m_MaxInfectionTime;
    float m_CurrentInfectionTime;
    bool m_Active;

    NodeConnection m_NodeConnection;

    protected override void Awake()
    {
        base.Awake();
        m_Active = false;
        m_CurrentInfectionTime = m_MaxInfectionTime;
        m_NodeConnection = GetComponent<NodeConnection>();

        if(!m_NodeConnection)
        {
            Debug.LogError("The infection timer for the " + this.name + " node connector couldn't find it's connector, this shouldn't happen!");
        }

        EventSystem.PowerConsumerActiveStateChangeHandler += CheckInfectionSpread;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        PowerConsumer power_consumer = GetComponent<PowerConsumer>();

        if(power_consumer)
        {
            m_Active = power_consumer.IsActive;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If we're a connection then we do nothing if we're off
        //If we're on then when the timer hits zero then check both nodes and if either are infected then infect the other based on the percent chance
        if (m_Active)
        {
            m_CurrentInfectionTime -= Time.deltaTime;

            if (m_CurrentInfectionTime <= 0.0f)
            {
                SpreadInfection();
            }
        }
    }

    void OnDestroy()
    {
        EventSystem.PowerConsumerActiveStateChangeHandler -= CheckInfectionSpread;
    }

    void SpreadInfection()
    {
        NodeInfectionTimer first_node_timer_comp = m_NodeConnection.StartNode.GetComponent<NodeInfectionTimer>();
        NodeInfectionTimer second_node_timer_comp = m_NodeConnection.EndNode.GetComponent<NodeInfectionTimer>();

        if (first_node_timer_comp && second_node_timer_comp)
        {
            if (first_node_timer_comp.IsInfected())
            {
                if (Random.value <= first_node_timer_comp.InfectionPercent)
                {
                    second_node_timer_comp.IncreaseInfection(first_node_timer_comp.InfectionIncreasePerSecond * Time.deltaTime);
                }
            }

            if (second_node_timer_comp.IsInfected())
            {
                if (Random.value <= second_node_timer_comp.InfectionPercent)
                {
                    first_node_timer_comp.IncreaseInfection(second_node_timer_comp.InfectionIncreasePerSecond * Time.deltaTime);
                }
            }
        }
    }

    //When we receive the PowerConsumerActiveStateChange event check to see if it was us. If it was then check to see if it was to on. 
    //If it was then reset the timer and check the attached nodes to see if either of them are infected. If either of them are then infect the other
    protected void CheckInfectionSpread(GameObject triggered_obj, bool is_power_on)
    {
        if (triggered_obj == this.gameObject)
        {
            if (is_power_on)
            {
                m_CurrentInfectionTime = m_MaxInfectionTime;
                SpreadInfection();
            }

            m_Active = is_power_on;
        }
    }

    //If we're a node then we will receive an event when we're infected with the % chance. If we aren't infected then infect and check our connections
}
