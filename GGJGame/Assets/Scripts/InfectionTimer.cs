using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InfectionTimer : MonoBehaviour
{
    [SerializeField]
    protected float m_InfectionPercentIncreasePerSecond;

    [SerializeField]
    protected float m_InfectionPercent;

    protected virtual void Awake()
    {
        //The percent increase comes in as a 0-100 value, but we want it as a 0-1 value
        m_InfectionPercentIncreasePerSecond *= 0.01f;
        m_InfectionPercent *= 0.01f;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseInfection(float percent_increase)
    {
        m_InfectionPercent = Mathf.Min(m_InfectionPercent + percent_increase, 1.0f);
        Button button = GetComponent<Button>();

        if(button)
        {
            ColorBlock button_color_block = button.colors;
            button_color_block.normalColor = new Color(m_InfectionPercent, 0.0f, 0.0f, 1.0f);
            button.colors = button_color_block;
        }
    }

    public float InfectionPercent
    {
        get
        {
            return m_InfectionPercent;
        }
    }

    public float InfectionIncreasePerSecond
    {
        get
        {
            return m_InfectionPercentIncreasePerSecond;
        }
    }
}
