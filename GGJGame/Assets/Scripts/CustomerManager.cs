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
    int m_CustomerQueueSize;
    [SerializeField]
    GameObject m_OrderTextPrefab;
    int m_NumQueuedCustomers;
    int m_LastGeneratedCustomerOrderIndex;
    Customer[] m_Customers;
    CustomerOrder[] m_CustomerOrders;
    Text[] m_OrderText;

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

        m_Customers = new Customer[m_CustomerQueueSize];
        m_NumQueuedCustomers = 0;
        m_LastGeneratedCustomerOrderIndex = 0;
        GenerateCustomerDataFromFile();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.PowerConsumerActiveStateChangeHandler += UpdateCustomerOrders;

        m_OrderText = new Text[m_CustomerQueueSize];
        text_comp = gameObject.GetComponent<Text>();
        GameObject canvas = FindObjectOfType<Canvas>().gameObject;

        for (int i = 0; i < m_CustomerQueueSize; i++)
        {
            GameObject text_obj = Instantiate(m_OrderTextPrefab, new Vector3(150, 450 - 60 * i), Quaternion.identity);
            text_obj.transform.SetParent(canvas.transform);
            m_OrderText[i] = text_obj.GetComponent<Text>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_NumQueuedCustomers < m_Customers.Length || Input.GetButtonDown("Spawn"))
        {
            SpawnCustomer();
        }
    }

    void SpawnCustomer()
    {
        //If we somehow get here and the customer orders wasn't filled out then fill it out
        if(m_CustomerOrders == null)
        {
            GenerateCustomerDataFromFile();
        }

        Debug.Log("Spawning a customer");
        GameObject customer = new GameObject();
        customer.name = "Customer";
        Customer customer_comp = customer.AddComponent<Customer>();

        if (customer_comp)
        {
            CustomerOrder order = m_CustomerOrders[m_LastGeneratedCustomerOrderIndex];
            customer_comp.SetupCustomer(order);
            m_LastGeneratedCustomerOrderIndex++;
            m_LastGeneratedCustomerOrderIndex %= m_CustomerOrders.Length;

            m_Customers[m_NumQueuedCustomers] = customer_comp;
            m_OrderText[m_NumQueuedCustomers].text = customer.name + " wants " + order.m_Resource + " wired to " + order.m_Destination + " for " + order.m_OrderDuration + " hours. They'll pay $" + order.m_Reward;
            m_NumQueuedCustomers++;
        }
    }
    
    public void FinishCustomer(Customer customer, bool completed_order)
    {
        //Find the passed customer and delete them and remove them from the array
        //Compress the array and dec the number of customers
        //Find the money meter and add the reward to it
        bool found_customer = false;

        for (int i = 0; i < m_NumQueuedCustomers; i++)
        {
            if (!found_customer)
            {
                if (customer == m_Customers[i])
                {
                    found_customer = true;

                    if (completed_order)
                    {
                        //TODO: Add the reward money here
                    }

                    Destroy(customer.gameObject);
                }
            }
            else
            {
                m_Customers[i - 1] = m_Customers[i];
                m_OrderText[i - 1].text = m_OrderText[i].text;
                m_OrderText[i].text = " ";
            }
        }

        if(found_customer)
        {
            m_NumQueuedCustomers--;
        }
    }

    void GenerateCustomerDataFromFile()
    {
        if (m_CustomerOrders != null)
        {
            return;
        }

        //Text file format:
        //The customer data file is a .tsv that has a different customer on each row
        //The format goes resource, location, duration, reward, narrative text
        char[] delimeters = { '\n', '\r' };
        string[] customer_order_lines = m_CustomerOrderFile.text.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
        m_CustomerOrders = new CustomerOrder[customer_order_lines.Length];

        for(int line_index = 0; line_index < customer_order_lines.Length; line_index++)
        {
            CustomerOrder order = new CustomerOrder();
            //For consistency and to not overload the player with a ton of data we're always making the order duration 10 seconds
            order.m_OrderDuration = 10.0f;
            string[] order_tokens = customer_order_lines[line_index].Split('\t');

            //I don't like how I'm parsing these, but it works and I can't think of anything else right now
            for (int token_index = 0; token_index < order_tokens.Length; token_index++)
            {
                string cur_token = order_tokens[token_index];

                if(cur_token == null || cur_token.Length <= 0)
                {
                    continue;
                }

                for (int string_index = 0; string_index < cur_token.Length; string_index++)
                {
                    if(cur_token[string_index] == ' ')
                    {
                        cur_token = cur_token.Remove(string_index, 1);
                        string_index--;
                    }
                }

                switch ((CustomerOrderParseFormat)token_index)
                {
                    case CustomerOrderParseFormat.Resource:
                    {
                        if(!Enum.TryParse(cur_token, out order.m_Resource))
                        {
                            Debug.LogError("We were unable to parse order " + line_index + "'s resource!");
                        }
                        break;
                    }
                    case CustomerOrderParseFormat.Location:
                    {
                        if(!Enum.TryParse(cur_token, out order.m_Destination))
                        {
                            Debug.LogError("We were unable to parse order " + line_index + "'s destination!");
                        }
                        break;
                    }
                    case CustomerOrderParseFormat.Duration:
                    {
                        if(!float.TryParse(cur_token, out order.m_OrderFulfillmentDuration))
                        {
                            Debug.LogError("We were unable to parse order " + line_index + "'s duration!");
                        }
                        break;
                    }
                    case CustomerOrderParseFormat.Reward:
                    {
                        if(!int.TryParse(cur_token, out order.m_Reward))
                        {
                            Debug.LogError("We were unable to parse order " + line_index + "'s reward!");
                        }
                        break;
                    }
                    case CustomerOrderParseFormat.Narrative:
                    {
                        order.m_Narrative = cur_token;
                        break;
                    }
                }
            }

            m_CustomerOrders[line_index] = order;
        }
    }

    public void UpdateCustomerOrders(GameObject triggered_obj, bool is_power_on)
    {
        for (int i = 0; i < m_NumQueuedCustomers; i++)
        {
            m_Customers[i].UpdateOrder();
        }
    }
}
