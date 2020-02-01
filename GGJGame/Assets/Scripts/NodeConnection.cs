using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeConnection : MonoBehaviour
{
    [SerializeField]
    NodeInterface m_NodeOne;
    [SerializeField]
    NodeInterface m_NodeTwo;
    bool m_Active;
    [SerializeField]
    int m_ActiveCost;

    void Awake()
    {
        //The PowerManager is a singleton so we're guaranteed to only get the correct one
        m_Active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Button button_comp = GetComponent<Button>();

        if(!button_comp)
        {
            Debug.LogError("We were unable to find the button component for " + this.name + "'s NodeConnection component!");
        }

        button_comp.onClick.AddListener(ConnectionOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ConnectionOnClick()
    {
        bool old_active = m_Active;

        if (m_Active)
        {
            m_Active = false;
            PowerManager.Instance.RestorePower(m_ActiveCost);
            Debug.Log(this.name + " turned this connection off!");
        }
        else
        {
            if (PowerManager.Instance.DrainPower(m_ActiveCost))
            {
                m_Active = true;
                Debug.Log(this.name + " had enough power to turn this connection on!");
            }
            else
            {
                Debug.Log(this.name + " didn't have enough power to turn on this connection!");
            }
        }

        if(old_active != m_Active)
        {
            CustomerManager.Instance.UpdateCustomerOrders();
        }
    }

    public NodeInterface GetOtherConnection(NodeInterface requesting_node)
    {
        if(m_NodeOne == requesting_node)
        {
            return m_NodeTwo;
        }
        else if(m_NodeTwo == requesting_node)
        {
            return m_NodeOne;
        }
        else
        {
            Debug.LogError(requesting_node.name + " tried getting the other node in connection " + this.name + " when it isn't a part of this connection!");
            return null;
        }
    }

    public bool ConnectionHasNode(NodeInterface requesting_node)
    {
        return m_NodeOne == requesting_node || m_NodeTwo == requesting_node;
    }
}
