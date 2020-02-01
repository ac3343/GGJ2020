using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSolver : Singleton<GraphSolver>
{
    NodeConnection[] m_AllConnections;
    ResourceNode[] m_AllResources;
    LocationNode[] m_AllLocations;

    public override void Awake()
    {
        base.Awake();
        m_AllConnections = FindObjectsOfType<NodeConnection>();
        m_AllResources = FindObjectsOfType<ResourceNode>();
        m_AllLocations = FindObjectsOfType<LocationNode>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool FindPath(NodeInterface start_node, NodeInterface end_node, bool include_off_paths)
    {
        return false;
    }

    public NodeConnection[] Connections
    {
        get
        {
            return m_AllConnections;
        }
    }

    public ResourceNode[] Resources
    {
        get
        {
            return m_AllResources;
        }
    }

    public LocationNode[] Locations
    {
        get
        {
            return m_AllLocations;
        }
    }
}
