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
        List<NodeInterface> _TouchedNodes = new List<NodeInterface>();
        _TouchedNodes.Add(start_node);
        Queue<NodeInterface> _Nodes = new Queue<NodeInterface>();

        foreach(NodeConnection c in start_node.Connections)
        {
            PowerConsumer power_consumer_comp = c.GetComponent<PowerConsumer>();

            if(!include_off_paths && (!power_consumer_comp || !power_consumer_comp.IsActive))
            {
                continue;
            }

            NodeInterface _OtherNode = c.GetOtherConnection(start_node);

            if(_OtherNode == end_node)
            {
                return true;
            }
            else
            {
                _Nodes.Enqueue(_OtherNode);
                _TouchedNodes.Add(_OtherNode);

            }
        }

        while(_Nodes.Count > 0)
        {
            NodeInterface _currentNode = _Nodes.Dequeue();

            foreach (NodeConnection c in _currentNode.Connections)
            {
                PowerConsumer power_consumer_comp = c.GetComponent<PowerConsumer>();

                if (!include_off_paths && (!power_consumer_comp || !power_consumer_comp.IsActive))
                {
                    continue;
                }

                NodeInterface _OtherNode = c.GetOtherConnection(_currentNode);

                if (_OtherNode == end_node)
                {
                    return true;
                }
                else if(!_TouchedNodes.Contains(_OtherNode))
                {
                    _Nodes.Enqueue(_OtherNode);
                    _TouchedNodes.Add(_OtherNode);
                }
            }
        }
        return false;
    }

    public bool FindPath(ResourceTypes resource_type, LocationTypes location_type, bool include_off_paths)
    {
        ResourceNode[] resources_to_check = new ResourceNode[m_AllResources.Length];
        int num_resources = 0;

        for (int i = 0; i < m_AllResources.Length; i++)
        {
            if(m_AllResources[i].ResourceType == resource_type)
            {
                resources_to_check[num_resources] = m_AllResources[i];
                num_resources++;
            }
        }

        LocationNode[] locations_to_check = new LocationNode[m_AllLocations.Length];
        int num_locations = 0;

        for (int i = 0; i < m_AllLocations.Length; i++)
        {
            if (m_AllLocations[i].LocationType == location_type)
            {
                locations_to_check[num_locations] = m_AllLocations[i];
                num_locations++;
            }
        }

        for (int resource_index = 0; resource_index < num_resources; resource_index++)
        {
            for (int location_index = 0; location_index < num_locations; location_index++)
            {
                if(FindPath(resources_to_check[resource_index], locations_to_check[location_index], include_off_paths))
                {
                    return true;
                }
            }
        }

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
