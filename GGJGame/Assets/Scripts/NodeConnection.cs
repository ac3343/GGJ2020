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

    // Start is called before the first frame update
    void Start()
    {
        //The PowerManager is a singleton so we're guaranteed to only get the correct one
        m_Active = false;
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
    }
}
