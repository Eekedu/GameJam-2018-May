using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEffector : MonoBehaviour {
    TokenScript.TokenType status = TokenScript.TokenType.TokenNone;
    public GameObject firePrefab, airPrefab, WaterPrefab, EarthPrefab, ElePrefab;
    private AudioSource AudSrc;
    public AudioClip fireAttackS, airAttackS, waterAttackS, EarthAttackS, EleAttackS;
    movement mmove;
    float nextGen, genTime;
    private bool doGen = false;
    Vector2 pos, dirGen;
    private void Start()
    {
        AudSrc = this.GetComponent<AudioSource>();
        mmove = this.GetComponent<movement>();
        mmove.setAnim(status);
    }

    public TokenScript.TokenType getStatus()
    {
        return status;
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
            case TokenScript.TokenType.TokenFire: {AudSrc.clip = fireAttackS;fireAttack(dir);
                } break;
            case TokenScript.TokenType.TokenWater: { AudSrc.clip = waterAttackS; waterAttack(dir); } break;
            case TokenScript.TokenType.TokenElectric: { AudSrc.clip = EleAttackS; lightAttack(dir); } break;
            case TokenScript.TokenType.TokenEarth: { AudSrc.clip = EarthAttackS; earthAttack(dir); } break;
            case TokenScript.TokenType.TokenWind: { AudSrc.clip = airAttackS; windAttack(dir); } break;
        }
        if (AudSrc.clip != null)
        {
            AudSrc.Play();
        }
    }

    void fireAttack(Vector2 dir) {
        GameObject newObj = Instantiate(firePrefab, new Vector3(this.transform.position.x, this.transform.position.y), Quaternion.identity);
        newObj.SendMessage("setOwner", this.gameObject);
        newObj.SendMessage("setVel", dir * 50);
    }
    void waterAttack(Vector2 dir) {
        dir.x *= 5f;
        GameObject newObj = Instantiate(WaterPrefab, new Vector3(this.transform.position.x, this.transform.position.y), Quaternion.identity);
        newObj.SendMessage("setOwner", this.gameObject);
        newObj.SendMessage("setVel", dir * 25);
    }
    void lightAttack(Vector2 dir) {
        pos = this.transform.position;
        dirGen = dir;
        doGen = true;
        nextGen = Time.fixedTime + 0.12f;
        genTime = Time.fixedTime + 1f;
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
                    pushBack *= 10f;
                    pushBack.y += 5f;
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
                if (Time.fixedTime >= genTime)
                {
                    doGen = false;
                }
                GameObject newObj = Instantiate(ElePrefab, new Vector3(pos.x, pos.y), Quaternion.identity);
                newObj.SendMessage("setOwner", this.gameObject);
                newObj.SendMessage("setVel", dirGen * 1200);
                nextGen = Time.fixedTime + 0.14f;
            }
        }
    }

    public TokenScript.TokenType[] goodAgainst()
    {
        TokenScript.TokenType[] types = new TokenScript.TokenType[2];
        switch (this.status)
        {
            case TokenScript.TokenType.TokenEarth:
                {
                    types[0] = TokenScript.TokenType.TokenFire;
                    break;
                }
            case TokenScript.TokenType.TokenElectric:
                {
                    types[0] = TokenScript.TokenType.TokenWater;
                    types[1] = TokenScript.TokenType.TokenWater;
                    break;
                }
            case TokenScript.TokenType.TokenFire:
                {
                    types[0] = TokenScript.TokenType.TokenWind;
                    types[1] = TokenScript.TokenType.TokenElectric;
                    break;
                }
            case TokenScript.TokenType.TokenWater:
                {
                    types[0] = TokenScript.TokenType.TokenFire;
                    types[1] = TokenScript.TokenType.TokenEarth;
                    break;
                }
            case TokenScript.TokenType.TokenWind:
                {
                    types[0] = TokenScript.TokenType.TokenEarth;
                    types[1] = TokenScript.TokenType.TokenElectric;
                    break;
                }
        }
        return types;
    }

    public TokenScript.TokenType badAgainst()
    {
        TokenScript.TokenType types = TokenScript.TokenType.TokenNone;
        switch (this.status)
        {
            case TokenScript.TokenType.TokenEarth:
                {
                    break;
                }
            case TokenScript.TokenType.TokenElectric:
                {
                    types = TokenScript.TokenType.TokenEarth;
                    break;
                }
            case TokenScript.TokenType.TokenFire:
                {
                    types = TokenScript.TokenType.TokenWater;
                    break;
                }
            case TokenScript.TokenType.TokenWater:
                {
                    types = TokenScript.TokenType.TokenElectric;
                    break;
                }
            case TokenScript.TokenType.TokenWind:
                {
                    types = TokenScript.TokenType.TokenFire;
                    break;
                }
        }
        return types;
    }
}
