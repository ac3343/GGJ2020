using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex
{
    TitleScene = 0,
    GameplayScene,
    EndScene,
    CreditsScene,

    SceneIndexCount
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    float m_DayLengthInSeconds;
    [SerializeField]
    int m_MinimumMoneyRequired;
    [SerializeField]
    float m_InfectionLosePercent;

    float m_CurrentDayLength;
    float m_InfectionPerc;
    int m_Money;
    bool m_GameWon;

    public override void Awake()
    {
        base.Awake();

        SceneManager.activeSceneChanged += OnSceneChange;

        m_InfectionLosePercent *= 0.01f;
        m_CurrentDayLength = m_DayLengthInSeconds;
        m_GameWon = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_CurrentDayLength -= Time.deltaTime;

        if(m_CurrentDayLength <= 0.0f)
        {
            EndTheDay();
        }

        if(InfectionManager.Instance.InfectionPercent >= m_InfectionLosePercent)
        {
            EndTheDay();
        }
    }

    void EndTheDay()
    {
        m_Money = CustomerManager.Instance.Money;
        m_InfectionPerc = InfectionManager.Instance.InfectionPercent;
        m_GameWon = m_Money >= m_MinimumMoneyRequired && m_InfectionPerc < m_InfectionLosePercent;
        SceneManager.LoadScene((int)SceneIndex.EndScene);
    }

    void OnSceneChange(Scene old_scene, Scene new_scene)
    {
        if(new_scene.buildIndex == (int)SceneIndex.GameplayScene)
        {
            m_CurrentDayLength = m_DayLengthInSeconds;
            m_GameWon = false;
        }
        else if(new_scene.buildIndex == (int)SceneIndex.EndScene)
        {
            EndSceneTextManager end_scene_manager = FindObjectOfType<EndSceneTextManager>();
            end_scene_manager.SetupEndScene(m_Money, m_InfectionPerc);
        }
    }

    public bool GameWon
    {
        get
        {
            return m_GameWon;
        }
    }
}
