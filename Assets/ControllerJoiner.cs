using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerJoiner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(m_sControlStart))
        {
            ToggleReady();
        }
        if (Input.GetButtonDown(m_sControlFire))
        {
            ToggleJoined();
        }
	}
    bool m_bReady;
    bool m_bJoined;


    string m_sControlPrefix;
    string m_sControlStart;
    string m_sControlFire;
    string m_sControlJump;

    const string c_sStartSuffix="Start";
    const string c_sFireSuffix="Fire";
    public void SetControllerNumber(int indexOne)
    {
        m_sControlPrefix = "P" + indexOne.ToString() + "_";
        string m_sControlStart = m_sControlPrefix+c_sStartSuffix;
        string m_sControlFire = m_sControlPrefix + c_sFireSuffix;
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
