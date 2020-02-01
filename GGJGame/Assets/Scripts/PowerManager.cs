using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : Singleton<PowerManager>
{
    [SerializeField]
    int m_MaxPowerAmount;
    int m_CurrentPowerAmount;

    void Awake()
    {
        m_CurrentPowerAmount = m_MaxPowerAmount;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool DrainPower(int amount)
    {
        Debug.Log("Someone requested " + amount + " power and we have " + m_CurrentPowerAmount + " power left");

        if(amount < m_CurrentPowerAmount)
        {
            m_CurrentPowerAmount -= amount;
            return true;
        }

        return false;
    }

    public void RestorePower(int amount)
    {
        Debug.Log("Someone restored " + amount + " power back to us");
        m_CurrentPowerAmount += amount;

        if (m_CurrentPowerAmount > m_MaxPowerAmount)
        {
            Debug.LogError("We somehow have more power than when we started!");
        }
    }
}
