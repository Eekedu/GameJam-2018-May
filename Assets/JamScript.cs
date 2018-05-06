using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamScript : MonoBehaviour {


    // Use this for initialization
    private float fSplashEnd;
    private bool m_bExiting = false;
    void Start()
    {
        fSplashEnd = Time.fixedTime + 6.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (!m_bExiting)
        {
            if (Time.fixedTime >= fSplashEnd || Input.anyKeyDown)
            {
                m_bExiting = true;
                SceneBoss.g_oSceneBoss.FadeOut();
            }
        }
        else
        {
            if (SceneBoss.g_oSceneBoss.FadeComplete())
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(SceneBoss.g_oSceneBoss.GetSceneIndex(SceneBoss.SceneSelect.SS_Title));
            }
        }
    }
}

