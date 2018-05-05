using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {

    Vector2[] m_v2PlayerSpawns;
    TokenSpawn[] m_TokenSpawns;
    public GameObject playerPrefab;
    public GameObject tokenPrefab;


    private class TokenSpawn
    {
        public Vector2 m_vPosition;
        public TokenScript.TokenType m_tType;
        public TokenScript m_oObject;
        public bool m_bSpawning;
        public float m_fSpawnTime;
    }

    private enum Phase
    {
        RP_Initial,
        RP_Starting,
        RP_Playing
    }

    private Phase m_pPhase = Phase.RP_Initial;
    private float m_fStartTime;
    private Text m_tOverlayText;

	// Use this for initialization
	void Start () {
        m_tOverlayText = GetComponentInChildren<Text>();
        SceneBoss.g_oSceneBoss.FadeIn();
        spawntimetest = Time.fixedTime + 1.0f;
        ConvertPlayerSpawns();
        ConvertTokenSpawns();
        EnterPhaseStart();
    }

    

    private void EnterPhaseStart()
    {
        m_pPhase = Phase.RP_Starting;
        m_fStartTime = Time.fixedTime + 3.1f;
    }
    private void ProcPhaseStart()
    {
        float fTimeToGo = m_fStartTime - Time.fixedTime;
        int iTimeToGo = (int)Mathf.FloorToInt(fTimeToGo);
        if (iTimeToGo > 0)
        {
            string sTimeToGo = iTimeToGo.ToString();
            m_tOverlayText.text = sTimeToGo + "...";
        } else
        {
            EnterPhasePlay();
        }
    }

    private void EnterPhasePlay()
    {
        m_pPhase = Phase.RP_Playing;
        m_tOverlayText.text = "Fight!";
        SpawnPlayer(1);
        m_fStartTime = Time.fixedTime + 1.0f;
    }
    private void ProcPhasePlay()
    {
        SpawnTokens();
        if (m_fStartTime >= Time.fixedTime)
        {
            m_tOverlayText.enabled = false;
        }
    }

    private void ConvertPlayerSpawns()
    {
        playerController[] pcspots = FindObjectsOfType<playerController>();
        m_v2PlayerSpawns = new Vector2[pcspots.Length];
        int ddex = 0;
        foreach (playerController pcon in pcspots)
        {
            m_v2PlayerSpawns[ddex] = new Vector2(pcon.transform.position.x, pcon.transform.position.y);
            Destroy(pcon.gameObject);
        }
    }

    private void ConvertTokenSpawns()
    {
        TokenScript[] tkspots = FindObjectsOfType<TokenScript>();
        m_TokenSpawns = new TokenSpawn[tkspots.Length];
        int ddex = 0;
        foreach (TokenScript tkspot in tkspots)
        {
            m_TokenSpawns[ddex] = new TokenSpawn();
            m_TokenSpawns[ddex].m_vPosition = new Vector2(tkspot.transform.position.x, tkspot.transform.position.y);
            m_TokenSpawns[ddex].m_tType = tkspot.GetTokenType();
            m_TokenSpawns[ddex].m_bSpawning = true;
            m_TokenSpawns[ddex].m_fSpawnTime = Time.fixedTime + 2.0f;
            Destroy(tkspot.gameObject);
        }
    }

    private void SpawnPlayer(int playerdex)
    {
        Instantiate(playerPrefab, new Vector3(m_v2PlayerSpawns[0].x, m_v2PlayerSpawns[0].y), Quaternion.identity);
    }

    private void SpawnTokens()
    {
        foreach (TokenSpawn tspawn in m_TokenSpawns)
        {
            if (tspawn.m_bSpawning)
            {
                if (Time.fixedTime>=tspawn.m_fSpawnTime)
                {
                    SpawnToken(tspawn);
                }
            }
        }
    }

    private void SpawnToken(TokenSpawn ttspawn)
    {
        TokenScript tscrip = Instantiate(tokenPrefab, new Vector3(ttspawn.m_vPosition.x, ttspawn.m_vPosition.y), Quaternion.identity).GetComponent<TokenScript>();
        ttspawn.m_oObject = tscrip;
        tscrip.SetTokenType(ttspawn.m_tType);
        ttspawn.m_bSpawning = false;
    }

    bool spawntest = true;
    float spawntimetest;
	// Update is called once per frame
	void Update () {
        switch (m_pPhase)
        {
            case Phase.RP_Starting:
                ProcPhaseStart();
                break;
            case Phase.RP_Playing:
                ProcPhasePlay();
                break;
        }
        //SpawnTokens();
    }
    public void GrabToken(TokenScript ttg)
    {
        foreach (TokenSpawn tspawn in m_TokenSpawns)
        {
            if (tspawn.m_oObject==ttg)
            {
                tspawn.m_bSpawning = true;
                tspawn.m_fSpawnTime = Time.fixedTime + 5.0f;
                Destroy(ttg.gameObject);
            }
        }
    }
}
