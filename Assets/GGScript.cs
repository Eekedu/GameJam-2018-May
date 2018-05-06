using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GGScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (SceneBoss.g_oSceneBoss != null) SceneBoss.g_oSceneBoss.FadeIn();
        GameManager gman = FindObjectOfType<GameManager>();
        Text[] txts = GetComponentsInChildren<Text>();
        txts[1].text = "Player " + gman.GetWinner().ToString();
        m_sWinnerFire = "P" + gman.GetWinner().ToString() + "_Fire";
        Destroy(gman);
	}

    string m_sWinnerFire;
    bool m_bLeaving = false;
	
	// Update is called once per frame
	void Update () {
        if (m_bLeaving) {
            if (SceneBoss.g_oSceneBoss!=null)
            {
                if (SceneBoss.g_oSceneBoss.FadeComplete())
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(SceneBoss.g_oSceneBoss.GetSceneIndex(SceneBoss.SceneSelect.SS_Title));
                }
            } else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
        }
        else
        {
            if (Input.GetButtonDown(m_sWinnerFire))
            {
                if (SceneBoss.g_oSceneBoss != null) SceneBoss.g_oSceneBoss.FadeOut();
                m_bLeaving = true;
            }
        }
	}
}
