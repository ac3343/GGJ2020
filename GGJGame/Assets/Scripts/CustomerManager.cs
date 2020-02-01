using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class determines when a new customer should be made
public class CustomerManager : Singleton<CustomerManager>
{
    [SerializeField]
    int m_MaxRewardAmount;
    List<Customer> m_Customers;

    private void Awake()
    {
        m_Customers = new List<Customer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Spawn"))
        {
            Debug.Log("Spawning a customer");
            GameObject customer = new GameObject();
            customer.name = "Customer";
            Customer customer_comp = customer.AddComponent<Customer>();
            
            if(customer_comp)
            {
                ResourceNode[] resources = GraphSolver.Instance.Resources;
                LocationNode[] locations = GraphSolver.Instance.Locations;
                int random_resource_index = Random.Range(0, resources.Length - 1);
                int random_location_index = Random.Range(0, resources.Length - 1);

                CustomerOrder order;
                order.m_Resource = resources[random_resource_index];
                order.m_Destination = locations[random_location_index];
                order.m_Reward = Random.Range(1, m_MaxRewardAmount);
                order.m_OrderDuration = 2.0f;
                order.m_OrderFulfillmentDuration = 0;

                customer_comp.SetupCustomer(order);
                m_Customers.Add(customer_comp);
            }
        }
    }

    public void UpdateCustomerOrders()
    {
        for(int i = 0;i<m_Customers.Count;i++)
        {
            m_Customers[i].UpdateOrder();
        }
    }
}
