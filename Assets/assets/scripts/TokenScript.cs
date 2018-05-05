using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenScript : MonoBehaviour {

    public enum TokenType { TokenEarth,TokenWind,TokenWater,TokenFire,TokenElectric,TokenNone}
    public TokenType MyType = TokenType.TokenEarth;
    private TokenType m_ttTokenType;

	// Use this for initialization
	void Start () {
<<<<<<< HEAD
        
=======
        m_ttTokenType = MyType;
>>>>>>> dd32010c4b4d99481833bfb598498b817f3f7928
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other == null) return;
        if (other.tag=="Player")
        {
            playerEffector effector = other.GetComponent<playerEffector>();
            effector.SetNearToken(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other == null) return;
        if (other.tag == "Player")
        {
            playerEffector effector = other.GetComponent<playerEffector>();
            effector.ClearNearToken(this);
        }
    }

    public TokenType GetTokenType()
    {
        return m_ttTokenType;
    }
    public void SetTokenType(TokenType settype)
    {
        m_ttTokenType = settype;
    }
}
