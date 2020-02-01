using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    private static PowerManager _instance;

    [SerializeField]
    int m_MaxPowerAmount;
    int m_CurrentPowerAmount;

    void Awake()
    {
        //Guarantee that there is only one instance of the PowerManager
        if(_instance != null && _instance != this)
        {
            Debug.LogError("There was a second instance of the PowerManager created, this should never happen!");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

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

    public static PowerManager Instance
    {
        get
        {
            if(_instance == null)
            {
                //Just in case the object actually exists, but we didn't have it
                _instance = FindObjectOfType<PowerManager>();

                if (_instance == null)
                {
                    GameObject power_manager_obj = new GameObject();
                    power_manager_obj.name = "PowerManager";
                    power_manager_obj.AddComponent<PowerManager>();
                }
            }

            return _instance;
        }
    }
}
