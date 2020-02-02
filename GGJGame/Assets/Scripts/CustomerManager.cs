using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

enum CustomerOrderParseFormat
{
    Resource = 0,
    Location,
    Duration,
    Reward,
    Narrative,

    OrderFormatCount,
}

//This class determines when a new customer should be made
public class CustomerManager : Singleton<CustomerManager>
{
    [SerializeField]
    TextAsset m_CustomerOrderFile;
    [SerializeField]
    int m_MaxRewardAmount;
    [SerializeField]
    int m_CustomerQueueSize;
    int m_NumQueuedCustomers;
    List<Customer> m_Customers;
    List<CustomerOrder> m_CustomerOrders;

    Text text_comp;

    private void Awake()
    {
        if(!m_CustomerOrderFile)
        {
            Debug.LogError("The customer order file is missing!");
        }

        if(m_CustomerQueueSize < 0)
        {
            Debug.LogError("The customer queue size was <= 0, this shouldn't happen!");
        }

        m_Customers = new List<Customer>(m_CustomerQueueSize);
        m_NumQueuedCustomers = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.PowerConsumerActiveStateChangeHandler += UpdateCustomerOrders;
        text_comp = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_NumQueuedCustomers < m_Customers.Capacity || Input.GetButtonDown("Spawn"))
        {
            SpawnCustomer();
        }
    }

    void SpawnCustomer()
    {

        StreamReader reaser = new StreamReader(m_CustomerOrderFile.)
        Debug.Log("Spawning a customer");
        GameObject customer = new GameObject();
        customer.name = "Customer";
        Customer customer_comp = customer.AddComponent<Customer>();

        if (customer_comp)
        {
            ResourceNode[] resources = GraphSolver.Instance.Resources;
            LocationNode[] locations = GraphSolver.Instance.Locations;
            int random_resource_index = Random.Range(0, resources.Length);

            CustomerOrder order;
            order.m_Resource = resources[random_resource_index];

            int order_attempts = 0;
            LocationNode random_location;
            do
            {
                order_attempts++;
                int random_location_index = Random.Range(0, locations.Length);
                random_location = locations[random_location_index];
            }
            while (!GraphSolver.Instance.FindPath(order.m_Resource, random_location, true) && order_attempts < resources.Length * 2);

            order.m_Destination = random_location;

            order.m_Reward = Random.Range(1, m_MaxRewardAmount);
            //TODO: this is hard coded, fix this at some point to not be
            order.m_OrderDuration = 5.0f;
            order.m_OrderFulfillmentDuration = 0;

            customer_comp.SetupCustomer(order);
            m_Customers.Add(customer_comp);

            text_comp.text = customer.name + " wants " + order.m_Resource + " wired to " + order.m_Destination + " for " + order.m_OrderDuration + " hours. They'll pay $" + order.m_Reward;
        }
    }

    void FindCustomerDataFromFile(int customer_number)
    {
        int file_char_index = 0;
        char curr_char = m_CustomerOrderFile.text[0];
        //Text file format:
        //The customer data file is a .tsv that has a different customer on each row
        //The format goes resource, location, duration, reward, narrative text
        string[] customer_order_lines = m_CustomerOrderFile.text.Split('\n');

        for(int i = 0;i<customer_order_lines.Length;i++)
        {
            CustomerOrder order;
            string[] order_tokens = customer_order_lines[i].Split('\t');

            //I don't like how I'm parsing these, but it works and I can't think of anything else right now
            for (int j = 0; j < order_tokens.Length; j++)
            {
                string cur_token = order_tokens[j];

                switch ((CustomerOrderParseFormat)j)
                {
                    case CustomerOrderParseFormat.Resource:
                    {
                        if(!Enum.TryParse(cur_token, out order.m_Destination))
                        {
                            Debug.LogError("We were unable to parse order " + i + " duration!");
                        }
                        break;
                    }
                    case CustomerOrderParseFormat.Location:
                    {
                        break;
                    }
                    case CustomerOrderParseFormat.Duration:
                    {
                        if(!float.TryParse(cur_token, out order.m_OrderFulfillmentDuration))
                        {
                            Debug.LogError("We were unable to parse order " + i + " duration!");
                        }
                        break;
                    }
                    case CustomerOrderParseFormat.Reward:
                    {
                        if(!int.TryParse(cur_token, out order.m_Reward))
                        {
                            Debug.LogError("We were unable to parse order " + i + " reward!");
                        }
                        break;
                    }
                    case CustomerOrderParseFormat.Narrative:
                    {
                        break;
                    }
                }
            }
        }

        while(m_CustomerOrderFile.text[file_char_index] != '\0')
        {
            if (m_CustomerOrderFile.text[file_char_index] == '\t')
            {
                string text_token = m_CustomerOrderFile.text.Split(Substring(0, file_char_index);
            }
            else
            {
                file_char_index++;
            }
        }
    }

    public void UpdateCustomerOrders(GameObject triggered_obj, bool is_power_on)
    {
        for (int i = 0; i < m_Customers.Count; i++)
        {
            m_Customers[i].UpdateOrder();
        }
    }
}
