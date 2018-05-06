using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEffector : MonoBehaviour {
    TokenScript.TokenType status = TokenScript.TokenType.TokenFire;
    public GameObject firePrefab, airPrefab, WaterPrefab, EarthPrefab, ElePrefab;
    movement mmove;
    float nextGen;
    private bool doGen = false;
    Vector2 pos, dirGen;
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

    void stopGen()
    {
        doGen = false;
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
        pos = this.transform.position;
        dirGen = dir;
        doGen = true;
        nextGen = Time.fixedTime + 0.2f;
    }
    void earthAttack(Vector2 dir) {
        dir.y += 10f;
        GameObject newObj = Instantiate(EarthPrefab, new Vector3(this.transform.position.x, this.transform.position.y), Quaternion.identity);
        newObj.SendMessage("setOwner", this.gameObject);
        newObj.SendMessage("setVel", dir * 50);
    }
    void windAttack(Vector2 dir) {
        RoundManager manager = FindObjectOfType<RoundManager>();
        RoundManager.ActivePlayer[] players = manager.GetPlayers();
        foreach (RoundManager.ActivePlayer player in players)
        {
            if (player.m_oObject != this.gameObject)
            {
                if (Vector2.Distance(player.GetPosition(), this.gameObject.transform.position) < 3f)
                {
                    Vector2 pushBack = Vector2.left * 1/(this.transform.position.x - player.GetPosition().x);
                    pushBack *= 50f;
                    player.m_oObject.GetComponent<movement>().addForce(pushBack);
                }
            }
        }
    }

    private void Update()
    {
        if (doGen)
        {
            if (Time.fixedTime >= nextGen)
            {
                GameObject newObj = Instantiate(ElePrefab, new Vector3(pos.x, pos.y), Quaternion.identity);
                newObj.SendMessage("setOwner", this.gameObject);
                newObj.SendMessage("setVel", dirGen);
                nextGen = Time.fixedTime + 0.2f;
            }
        }
    }
}
