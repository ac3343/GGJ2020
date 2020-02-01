﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerConsumer : MonoBehaviour
{
    [SerializeField]
    int m_ActiveCost;

    bool m_Active;
    Image img_comp;

    // Start is called before the first frame update
    void Awake()
    {
        m_Active = false;
    }

    private void Start()
    {
        img_comp = GetComponent<Image>();
        Button button_comp = GetComponent<Button>();

        if (!button_comp)
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
            img_comp.color = Color.white;
            PowerManager.Instance.RestorePower(m_ActiveCost);
            Debug.Log(this.name + " turned this consumer off!");
        }
        else
        {
            if (PowerManager.Instance.DrainPower(m_ActiveCost))
            {
                m_Active = true;
                img_comp.color = Color.green;
                Debug.Log(this.name + " had enough power to turn this consumer on!");
            }
            else
            {
                Debug.Log(this.name + " didn't have enough power to turn on this consumer!");
            }
        }

        if (old_active != m_Active)
        {
            EventSystem.Instance.OnPowerConsumerActiveStateChange();
        }
    }

    public void MouseEnter()
    {
        if(!m_Active)
            img_comp.color = Color.green;
    }

    public void MouseExit()
    {
        if (!m_Active)
            img_comp.color = Color.white;
    }

    public bool IsActive
    {
        get { return m_Active; }
    }
}
