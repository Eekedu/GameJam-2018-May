using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerJoiner : MonoBehaviour {


    Text m_tTitle;
    Text m_tJoined;
    Text m_tReady;

	// Use this for initialization
	void Start () {
        Text[] txts = GetComponentsInChildren<Text>();
        m_tTitle = txts[0];
        m_tJoined = txts[1];
        m_tReady = txts[2];
    }

    // Update is called once per frame
    void Update () {
        m_tTitle.text = "Controller: " + m_sControlPrefix;// m_iControllerIndex.toString();
        Debug.Log(m_sControlJump);
		if (Input.GetButtonDown(m_sControlJump))
        {
            ToggleReady();
        }
        if (Input.GetButtonDown(m_sControlFire))
        {
            ToggleJoined();
        }
        m_tJoined.text = m_bJoined ? "Connected!" : "Not connected.";
        m_tReady.text = m_bReady ? "Ready!" : "Not ready...";
	}
    bool m_bReady;
    bool m_bJoined;


    string m_sControlPrefix;
    string m_sControlStart;
    string m_sControlFire;
    string m_sControlJump;

    const string c_sJumpSuffix="Jump";
    const string c_sFireSuffix="Fire";
    public void SetControllerNumber(int indexOne)
    {
        m_sControlPrefix = "P" + indexOne.ToString() + "_";
        m_sControlJump = m_sControlPrefix+ c_sJumpSuffix;
        m_sControlFire = m_sControlPrefix + c_sFireSuffix;
       // Debug.Log(m_sControlJump);
    }

    private void ToggleReady()
    {
        if (!m_bJoined)
        {
            m_bReady = false;

        } else
        {
            m_bReady = !m_bReady;
        }
    }

    private void ToggleJoined()
    {
        m_bJoined = !m_bJoined;
        if (!m_bJoined)
        {
            m_bReady = false;
        }
    }
}
