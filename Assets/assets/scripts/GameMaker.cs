using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaker : MonoBehaviour {


    public ControllerJoiner prefControllerJoiner;

    private ControllerJoiner[] m_aControllerJoiners;

    enum Phase
    {
        PhaseWaiting,
        PhaseCounting,
        PhaseStarting
    }
    private Phase m_pPhase;

    private float m_fLaunchTime;

    private Text m_tText;
    private GameManager m_oManager;
	// Use this for initialization
	void Start () {
        if (SceneBoss.g_oSceneBoss!=null) SceneBoss.g_oSceneBoss.FadeIn();
        m_oManager = FindObjectOfType<GameManager>();

        m_tText = GetComponentInChildren<Text>();
        float fXStart = Screen.width * -0.75f;
        float fXStep = Screen.width * 0.5f;
        float fYStart = Screen.height * -0.55f;
        float fYStep = Screen.height * 0.5f;
        int iControllerIndex = 0;
        m_aControllerJoiners = new ControllerJoiner[4];
        for (int Y = 0; Y < 2; Y++)
        {
            for (int X = 0; X < 2; X++)
            {
                ControllerJoiner cjoin = Instantiate(prefControllerJoiner, this.transform);
                m_aControllerJoiners[iControllerIndex] = cjoin;
                iControllerIndex++;
                cjoin.gameObject.transform.localPosition = new Vector3(fXStart + (fXStep * (X + 1)), fYStart - (fYStep * (Y - 1)));
                cjoin.SetControllerNumber(iControllerIndex);
            }
        }
        EnterPhaseWaiting();
	}
	
	// Update is called once per frame
	void Update () {
		switch (m_pPhase)
        {
            case (Phase.PhaseWaiting):
                ProcPhaseWaiting();
                break;
            case (Phase.PhaseCounting):
                ProcPhaseCounting();
                break;
            case (Phase.PhaseStarting):
                ProcPhaseStarting();
                break;
        }
    }
    void EnterPhaseWaiting()
    {
        m_pPhase = Phase.PhaseWaiting;
    }
    void ProcPhaseWaiting()
    {
        m_tText.text = "Press Fire to join\nPress Jump to ready";
        if (CanStart())
        {
            EnterPhaseCounting();
        }
    }

    private bool CanStart()
    {
        return (MinimumJoined() && AllReady());
    }
    private bool MinimumJoined()
    {
        int iResult = 0;
        foreach (ControllerJoiner cjoiner in m_aControllerJoiners)
        {
            if (cjoiner.IsJoined())
            {
                iResult++;
            }
        }
        return iResult >= 1;
    }
    private bool AllReady()
    {
        bool bResult = true;
        foreach (ControllerJoiner cjoiner in m_aControllerJoiners)
        {
            if (!cjoiner.IsReady()) bResult = false;
        }
        return bResult;
    }
    void EnterPhaseCounting()
    {
        m_pPhase = Phase.PhaseCounting;
        m_fLaunchTime = Time.fixedTime + 3.0f;
    }
    void ProcPhaseCounting()
    {
        if (!CanStart()) EnterPhaseWaiting();
        if (Time.fixedTime>m_fLaunchTime)
        {
            m_tText.text = "Launching!";
            EnterPhaseStarting();
        } else
        {
            int iCountLeft = Mathf.CeilToInt(m_fLaunchTime - Time.fixedTime);
            m_tText.text = "Launching in " + iCountLeft.ToString() + "...";
        }

    }
    void EnterPhaseStarting()
    {
        m_pPhase = Phase.PhaseStarting;
        if (SceneBoss.g_oSceneBoss!=null)
        {
            SceneBoss.g_oSceneBoss.FadeOut();
        }
        for (int iterator = 0; iterator < 4; iterator++)
        {
            m_oManager.SetControllerActive(iterator,m_aControllerJoiners[iterator].IsJoined());
        }
        DontDestroyOnLoad(m_oManager.gameObject);
    }
    void ProcPhaseStarting()
    {
        if (SceneBoss.g_oSceneBoss != null)
        {
            if (SceneBoss.g_oSceneBoss.FadeComplete())
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(SceneBoss.g_oSceneBoss.GetSceneIndex(SceneBoss.SceneSelect.SS_Test));
            }
        } else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }
    }


}
