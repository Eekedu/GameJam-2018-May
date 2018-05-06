using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEffector : MonoBehaviour {
    TokenScript.TokenType status = TokenScript.TokenType.TokenFire;
    public GameObject firePrefab, airPrefab, WaterPrefab, EarthPrefab, ElePrefab;
    movement mmove;
    private void Start()
    {
        mmove = this.GetComponent<movement>();
        mmove.setAnim(status);
    }


    void pickup(int type)
    {
        if (m_lastToken == null) return;
        status = m_lastToken.GetTokenType() ;
        FindObjectOfType<RoundManager>().GrabToken(m_lastToken, this.gameObject);
        mmove.setAnim(status);

    }

    TokenScript m_lastToken;
    public void SetNearToken(TokenScript nuToken)
    {
        m_lastToken = nuToken;
    }

    public void ClearNearToken(TokenScript removeToken)
    {
        m_lastToken = null;
    }
    void attack(Vector2 dir)
    {
        switch (status)
        {
            case TokenScript.TokenType.TokenFire: { fireAttack(dir); } break;
            case TokenScript.TokenType.TokenWater: { waterAttack(dir); } break;
            case TokenScript.TokenType.TokenElectric: { lightAttack(dir); } break;
            case TokenScript.TokenType.TokenEarth: { earthAttack(dir); } break;
            case TokenScript.TokenType.TokenWind: { windAttack(dir); } break;
        }
    }

    void fireAttack(Vector2 dir) {
        GameObject newObj = Instantiate(firePrefab, new Vector3(this.transform.position.x, this.transform.position.y), Quaternion.identity);
        newObj.SendMessage("setOwner", this.gameObject);
        newObj.SendMessage("setVel", dir * 50);
    }
    void waterAttack(Vector2 dir) {

        GameObject newObj = Instantiate(WaterPrefab, new Vector3(this.transform.position.x, this.transform.position.y), Quaternion.identity);
        newObj.SendMessage("setOwner", this.gameObject);
        newObj.SendMessage("setVel", dir * 25);
    }
    void lightAttack(Vector2 dir) {
        for (int i = -1; i < 2; i++)
        {
            Vector2 tempdir = dir;
            tempdir.y = i * 15;
            GameObject newObj = Instantiate(ElePrefab, new Vector3(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            newObj.SendMessage("setOwner", this.gameObject);
            newObj.SendMessage("setVel", dir * 80);
        }
    }
    void earthAttack(Vector2 dir) { }
    void windAttack(Vector2 dir) {
        RoundManager.ActivePlayer[] players = FindObjectOfType<RoundManager>().GetPlayers();
        foreach (RoundManager.ActivePlayer player in players)
        {
            if (player.m_oObject != this.gameObject)
            {
                if (Vector2.Distance(player.GetPosition(), this.gameObject.transform.position) < 50)
                {
                    Debug.Log("WAT");
                }
            }
        }
    }

    private void Update()
    {
        
    }
}
