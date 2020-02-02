using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneTextManager : MonoBehaviour
{
    [SerializeField]
    Text m_TitleText;
    [SerializeField]
    Text m_MoneyText;
    [SerializeField]
    Text m_InfectionText;

    void Awake()
    {
        if(!m_TitleText)
        {
            Debug.LogError("The title text is missing for the end scene manager!");
        }

        if (!m_MoneyText)
        {
            Debug.LogError("The money text is missing for the end scene manager!");
        }

        if (!m_InfectionText)
        {
            Debug.LogError("The infection text is missing for the end scene manager!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupEndScene(float money, float infection_perc)
    {
        if (GameManager.Instance.GameWon)
        {
            m_TitleText.text = "You Win!";
            m_TitleText.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        }
        else
        {
            m_TitleText.text = "GAME OVER.";
            m_TitleText.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        m_MoneyText.text = "Final Money: " + money;
        m_InfectionText.text = "Total Infection percent: " + infection_perc * 100 + "%";
    }
}
