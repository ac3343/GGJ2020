using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceTypes
{
    Water = 0,
    Heating,
    Safety,
    Medicine,

    ResourceTypeCount
}

public class ResourceNode : NodeInterface
{
    [SerializeField]
    ResourceTypes m_ResourceType;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void NodeOnClick()
    {
        base.NodeOnClick();
        Debug.Log("Clicked on a Resource Node.");
    }

    public ResourceTypes ResourceType
    {
        get
        {
            return m_ResourceType;
        }
    }
}
