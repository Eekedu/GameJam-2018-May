using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDScript : MonoBehaviour {


    Text m_tTitle;
    Text m_tHealth;
    Text m_tStocks;
    Image m_iToken;

    float m_fFlashScale=2.0f;
    float m_fFlashTime;

    public Sprite m_preEarthToken;
    public Sprite m_preWindToken;
    public Sprite m_preWaterToken;
    public Sprite m_preFireToken;
    public Sprite m_preElectroToken;

    private const string smiley = "☺";
    // Use this for initialization
    void Start () {
        Text[] textitems = GetComponentsInChildren<Text>();
        m_tTitle = textitems[0];
        m_tHealth = textitems[1];
        m_tStocks = textitems[2];
        m_iToken = GetComponentInChildren<Image>();

        //SetPlayerTitle("Player 1");
        //SetHealth(300,false);
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
        if (m_tTitle == null) m_tTitle = GetComponentsInChildren<Text>()[0];
        m_tTitle.text = text;
    }

    public void SetHealth(float fHPLevel, bool flash)
    {
        int iHPLevel = Mathf.CeilToInt(fHPLevel);
        m_tHealth.text = iHPLevel.ToString();
        if (flash)
        {
            m_fFlashTime = Time.fixedTime + 0.5f;
        }
    }
    public void SetStocks(int iCount)
    {
        m_tStocks.text = "";
        for (int i=0;i<iCount;i++)
        {
            m_tStocks.text = m_tStocks.text + smiley;
        }
    }

    public void SetToken(TokenScript.TokenType ttype)
    {
        switch (ttype)
        {
            case TokenScript.TokenType.TokenNone:
                m_iToken.sprite = null;
                break;
            case TokenScript.TokenType.TokenEarth:
                m_iToken.sprite = m_preEarthToken;
                break;
            case TokenScript.TokenType.TokenWind:
                m_iToken.sprite = m_preWindToken;
                break;
            case TokenScript.TokenType.TokenWater:
                m_iToken.sprite = m_preWaterToken;
                break;
            case TokenScript.TokenType.TokenFire:
                m_iToken.sprite = m_preFireToken;
                break;
            case TokenScript.TokenType.TokenElectric:
                m_iToken.sprite = m_preElectroToken;
                break;

        }
    }
}
