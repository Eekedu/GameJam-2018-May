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
        newObj.SendMessage("setVel", Vector2.right * 10);
    }
    void waterAttack(Vector2 dir) { }
    void lightAttack(Vector2 dir) { }
    void earthAttack(Vector2 dir) { }
    void windAttack(Vector2 dir) { }

    private void Update()
    {
        
    }
}
