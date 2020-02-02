using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Technically unnecessary since I can't think of a reason why we would want multiples of the same location. This makes parsing way easier though
enum LocationTypes
{
    Housing = 0,
    Hospital,
    LifeSupport,
    Recreational,
    School,
    GreenHouse,

    LocationTypeCount
}

public class LocationNode : NodeInterface
{
    Image img_comp;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        img_comp = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        bool connections_active = false;

        foreach(NodeConnection c in m_Connections)
        {
            if (c.gameObject.GetComponent<PowerConsumer>().IsActive)
            {
                connections_active = true;
                break;
            }
        }
        if (connections_active)
            img_comp.color = Color.green;
        else
            img_comp.color = Color.white;

    }

    protected override void NodeOnClick()
    {
        base.NodeOnClick();
        Debug.Log("Clicked on a Location Node");
    }
}
