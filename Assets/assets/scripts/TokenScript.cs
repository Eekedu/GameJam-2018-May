using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenScript : MonoBehaviour {

    public enum TokenType { TokenEarth,TokenWind,TokenWater,TokenFire,TokenElectric,TokenNone}
    public TokenType MyType = TokenType.TokenEarth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("haw");
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
        return TokenType.TokenEarth;
    }
}
