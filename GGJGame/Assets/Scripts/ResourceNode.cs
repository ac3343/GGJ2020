using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : NodeInterface
{
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
        Debug.Log("Clicked on a Resource Node");
    }
}
