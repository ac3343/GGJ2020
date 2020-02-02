using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionManager : Singleton<InfectionManager>
{
    [SerializeField]
    float m_InfectPercent;
    NodeInfectionTimer[] m_NodeInfections;
    Scrollbar m_ScrollbarInfo;

    public override void Awake()
    {
        base.Awake();
        m_NodeInfections = FindObjectsOfType<NodeInfectionTimer>();
        m_InfectPercent = 0.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_ScrollbarInfo = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_NodeInfections.Length > 0)
        {
            float infectTotal = 0;

            for (int i = 0; i < m_NodeInfections.Length; i++)
            {
                infectTotal += m_NodeInfections[i].InfectionPercent;
            }

            m_InfectPercent = infectTotal / m_NodeInfections.Length;

            m_ScrollbarInfo.size = m_InfectPercent;
        }
    }

    public float InfectionPercent
    {
        get
        {
            return m_InfectPercent;
        }
    }
}
