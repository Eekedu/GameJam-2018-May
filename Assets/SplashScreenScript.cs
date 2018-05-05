using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenScript : MonoBehaviour {

    // Use this for initialization
    private float fSplashEnd;
    private bool m_bExiting = false;
	void Start () {
        fSplashEnd = Time.fixedTime + 3.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_bExiting)
        {
            if (Time.fixedTime>=fSplashEnd || Input.anyKeyDown)
            {
                m_bExiting = true;
                SceneBoss.g_oSceneBoss.FadeOut();
            }
        } else
        {
            if (SceneBoss.g_oSceneBoss.FadeComplete())
            {
                Debug.Log("Fin.");
            }
        }
	}
}
