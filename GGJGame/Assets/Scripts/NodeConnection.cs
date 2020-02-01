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
    [SerializeField]
    bool m_IsDirectional;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public NodeInterface GetOtherConnection(NodeInterface requesting_node)
    {
        if(m_NodeOne == requesting_node)
        {
            return m_NodeTwo;
        }
        else if(m_NodeTwo == requesting_node)
        {
            //If the connection is a directional connection then we will always have the connection going to node two, not node one
            if (!m_IsDirectional)
            {
                return m_NodeOne;
            }
        }
        else
        {
            Debug.LogError(requesting_node.name + " tried getting the other node in connection " + this.name + " when it isn't a part of this connection!");
            return null;
        }

        return null;
    }

    public bool ConnectionHasNode(NodeInterface requesting_node)
    {
        return m_NodeOne == requesting_node || m_NodeTwo == requesting_node;
    }
}
