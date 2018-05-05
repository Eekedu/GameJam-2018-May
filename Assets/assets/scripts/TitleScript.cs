﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneBoss.g_oSceneBoss.FadeIn();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_bLoadingScene)
        {
            if (SceneBoss.g_oSceneBoss.FadeComplete())
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(3);
            }
        }
	}

    private bool m_bLoadingScene = false;
    public void PlayButtonClick()
    {
        if (m_bLoadingScene) return;
        SceneBoss.g_oSceneBoss.FadeOut();
        m_bLoadingScene = true;
    }
}