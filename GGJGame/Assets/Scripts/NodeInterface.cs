using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NodeInterface : MonoBehaviour
{
    protected List<NodeConnection> m_Connections;
    protected float m_VirusPercent;

    protected virtual void Start()
    {
        Button button_comp = GetComponent<Button>();

        if (!button_comp)
        {
            Debug.LogError("We were unable to find the button component for " + this.name + "'s NodeInterface component!");
        }

        button_comp.onClick.AddListener(NodeOnClick);

        NodeConnection[] all_connections = GraphSolver.Instance.Connections;
        m_Connections = new List<NodeConnection>();

        //This is pretty slow, but it prevents weird bugs from not connecting things correctly
        for (int i = 0; i < all_connections.Length; i++)
        {
            if(all_connections[i].ConnectionHasNode(this))
            {
                m_Connections.Add(all_connections[i]);
            }
        }
    }

    protected virtual void NodeOnClick()
    {
        string connection_names = "Current connections: ";

        for (int i = 0; i < m_Connections.Count; i++)
        {
            NodeInterface connected_node = m_Connections[i].GetOtherConnection(this);

            if (connected_node)
            {
                connection_names += "'" + connected_node.name + "' ";
            }
        }

        Debug.Log(connection_names);
    }

    public List<NodeConnection> Connections
    {
        get { return m_Connections; }
    }

    public float VirusPercent
    {
        get
        {
            return m_VirusPercent;
        }
    }
}
