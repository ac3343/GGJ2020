using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CustomerOrder
{
    public ResourceTypes m_Resource;
    public LocationTypes m_Destination;
    public float m_OrderDuration;
    public float m_OrderFulfillmentDuration;
    public int m_Reward;
    public string m_Narrative;

    public bool IsValid()
    {
        //The order fulfillment duration can be zero or less, that just auto completes the order when the player satisfies the conditions
        return (m_Resource >= 0 && m_Resource < ResourceTypes.ResourceTypeCount) 
            && (m_Destination >= 0 && m_Destination < LocationTypes.LocationTypeCount) 
            && m_OrderDuration > 0 && m_Reward > 0;
    }
}

public class Customer : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    CustomerOrder m_Order;
    float m_CurrentOrderDuration;
    float m_CurrentOrderFulfillmentDuration;
    bool m_OrderSatisfied;

    void Awake()
    {
        m_CurrentOrderDuration = 0.0f;
        m_CurrentOrderFulfillmentDuration = 0.0f;
        m_OrderSatisfied = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_OrderSatisfied)
        {
            m_CurrentOrderDuration -= Time.deltaTime;

            if (m_CurrentOrderDuration <= 0.0f)
            {
                Debug.Log("The player took to long to satisfy my order!");
                CustomerManager.Instance.FinishCustomer(this, false);
            }
        }
        else
        {
            m_CurrentOrderFulfillmentDuration -= Time.deltaTime;

            if(m_CurrentOrderFulfillmentDuration <= 0.0f)
            {
                Debug.Log("The player fulfilled my order!");
                CustomerManager.Instance.FinishCustomer(this, true);
            }
        }
    }

    public void SetupCustomer(CustomerOrder order)
    {
        if(!order.IsValid())
        {
            Debug.LogError("The customer was given a bad order when it was set up! The order is " + order);
        }

        m_Order = order;
        m_CurrentOrderDuration = m_Order.m_OrderDuration;
        m_CurrentOrderFulfillmentDuration = m_Order.m_OrderFulfillmentDuration;

        UpdateOrder();
    }

    public void UpdateOrder()
    {
        m_OrderSatisfied = GraphSolver.Instance.FindPath(m_Order.m_Resource, m_Order.m_Destination, false);
    }

    public CustomerOrder Order
    {
        get
        {
            return m_Order;
        }
    }

    public bool ProcessingOrder
    {
        get { return m_OrderSatisfied; }
    }
}
