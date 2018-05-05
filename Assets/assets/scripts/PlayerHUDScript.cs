using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDScript : MonoBehaviour {


    Text m_tTitle;
    Text m_tHealth;

    float m_fFlashScale=2.0f;
    float m_fFlashTime;
	// Use this for initialization
	void Start () {
        Text[] textitems = GetComponentsInChildren<Text>();
        m_tTitle = textitems[0];
        m_tHealth = textitems[1];

        SetPlayerTitle("Player 1");
        SetHealth(300,false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.fixedTime <= m_fFlashTime)
        {
            float fScalar = 1.0f + ((m_fFlashTime - Time.fixedTime) * m_fFlashScale);
            m_tHealth.transform.localScale = new Vector3(fScalar, fScalar);
        } else
        {
            m_tHealth.transform.localScale = new Vector3(1.0f, 1.0f);

        }
    }

    public void SetPlayerTitle(string text)
    {
        m_tTitle.text = text;
    }

    public void SetHealth(int iHPLevel, bool flash)
    {
        m_tHealth.text = iHPLevel.ToString();
        if (flash)
        {
            m_fFlashTime = Time.fixedTime + 0.5f;
        }
    }
}
