using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NodeInterface : MonoBehaviour
{
    [SerializeField]
    NodeConnection[] m_Connections;

    protected virtual void Start()
    {
        Button button_comp = GetComponent<Button>();

        if (!button_comp)
        {
            Debug.LogError("We were unable to find the button component for " + this.name + "'s NodeInterface component!");
        }

        button_comp.onClick.AddListener(NodeOnClick);
    }

    protected abstract void NodeOnClick();
}
