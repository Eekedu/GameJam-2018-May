using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {

    Vector2[] m_v2PlayerSpawns;
    TokenSpawn[] m_TokenSpawns;
    public GameObject playerPrefab;
    public GameObject tokenPrefab;
    public TokenScript preEarthToken;
    public TokenScript preWindToken;
    public TokenScript preFireToken;
    public TokenScript preWaterToken;
    public TokenScript preElectroToken;
    public PlayerHUDScript m_preHUD;

    private int m_iPlayerCount;
    private PlayerHUDScript[] m_aoPlayerHUDs;
    private CameraScript m_oGameCamera;
    private ActivePlayer[] m_oActivePlayers;



    private class TokenSpawn
    {
        public Vector2 m_vPosition;
        public TokenScript.TokenType m_tType;
        public TokenScript m_oObject;
        public bool m_bSpawning;
        public float m_fSpawnTime;
    }

    private class ActivePlayer
    {
        public GameObject m_oObject;
        public playerEffector m_oEffector;
        public playerController m_oController;
        public ActivePlayer(GameObject playob)
        {
            m_oObject = playob;
            m_oController = playob.GetComponent<playerController>();
            m_oEffector = playob.GetComponent<playerEffector>();
        }
        public Vector2 GetPosition()
        {
            return new Vector2(m_oObject.transform.position.x, m_oObject.transform.position.y);
        }
        public void AssignPlayerNumber(int dex)
        {
            m_oController.AssignPlayerNumber(dex);
        }
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
        if (SceneBoss.g_oSceneBoss!=null) SceneBoss.g_oSceneBoss.FadeIn();
        ConvertPlayerSpawns();
        ConvertTokenSpawns();
        EnterPhaseStart();
        SetPlayerCount(2);
        m_oGameCamera = GetComponentInChildren<CameraScript>();
        


    }

    public void SetPlayerCount(int count)
    {
        float fXStart = Screen.width * -0.5f;
        float fYStart = (Screen.height * - 0.5f) + 75;
        m_iPlayerCount = count;
        m_aoPlayerHUDs = new PlayerHUDScript[count];
        float fHUDStep = Screen.width / (count + 1);
        Debug.Log("<" + fXStart.ToString() + "> <" + fYStart.ToString() + "> <" + fHUDStep.ToString() + ">");
        for (int i = 0; i < count; i++)
        {
            float fXPos = fXStart + ((i+1) * (fHUDStep));
            //m_aoPlayerHUDs[i] = Instantiate(m_preHUD, new Vector3(fPos, 55), Quaternion.identity,this.transform);
            m_aoPlayerHUDs[i] = Instantiate(m_preHUD, this.transform);
            m_aoPlayerHUDs[i].transform.localPosition = new Vector3(fXPos, fYStart);
        }
        m_oActivePlayers = new ActivePlayer[count];
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
        SpawnPlayer(2);
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
            Debug.Log("Consumed " + tkspot.GetTokenType().ToString());
            m_TokenSpawns[ddex].m_bSpawning = true;
            m_TokenSpawns[ddex].m_fSpawnTime = Time.fixedTime + 2.0f;
            Destroy(tkspot.gameObject);
            ddex++;
        }
        Debug.Log("Created " + ddex.ToString() + " token spawns.");
    }

    private void SpawnPlayer(int playerdex)
    {
        m_oActivePlayers[playerdex-1] = new ActivePlayer(Instantiate(playerPrefab, new Vector3(m_v2PlayerSpawns[playerdex - 1].x, m_v2PlayerSpawns[playerdex - 1].y), Quaternion.identity));
        m_aoPlayerHUDs[playerdex-1].SetHealth(100, false);
        m_oActivePlayers[playerdex - 1].AssignPlayerNumber(playerdex);
    }

    private float fHealth = 100f;
    private void DamagePlayer(int playerdex, float fDamage)
    {
        fHealth -= fDamage;
        m_aoPlayerHUDs[playerdex - 1].SetHealth(fHealth, true);
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
        TokenScript targetToken=null;
        switch (ttspawn.m_tType)
        {
            case TokenScript.TokenType.TokenEarth: targetToken = preEarthToken; break;
            case TokenScript.TokenType.TokenWind: targetToken = preWindToken; break;
            case TokenScript.TokenType.TokenWater: targetToken = preWaterToken; break;
            case TokenScript.TokenType.TokenFire: targetToken = preFireToken; break;
            case TokenScript.TokenType.TokenElectric: targetToken = preElectroToken; break;

        }
        if (targetToken == null) return;

        TokenScript tscrip = Instantiate(targetToken, new Vector3(ttspawn.m_vPosition.x, ttspawn.m_vPosition.y), Quaternion.identity).GetComponent<TokenScript>();
        ttspawn.m_oObject = tscrip;
        tscrip.SetTokenType(ttspawn.m_tType);
        ttspawn.m_bSpawning = false;
    }

    private Vector2 m_vTopLeftBound, m_vBottomRightBound;
    private void UpdatePlayerBounds()
    {
        m_vTopLeftBound = new Vector2();
        m_vBottomRightBound = new Vector2();
        foreach (ActivePlayer player in m_oActivePlayers)
        {
            m_vTopLeftBound.x = Mathf.Min(player.GetPosition().x, m_vTopLeftBound.x);
            m_vTopLeftBound.y = Mathf.Max(player.GetPosition().y, m_vTopLeftBound.y);
            m_vBottomRightBound.x = Mathf.Max(player.GetPosition().x, m_vBottomRightBound.x);
            m_vBottomRightBound.y = Mathf.Min(player.GetPosition().y, m_vBottomRightBound.y);
        }
    }


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
        if (Input.GetKeyDown(KeyCode.H))
        {
            DamagePlayer(1, 15);
        }
        UpdatePlayerBounds();
        m_oGameCamera.SetBounds(m_vTopLeftBound, m_vBottomRightBound);
        //SpawnTokens();
    }
    public void GrabToken(TokenScript ttg)
    {
        foreach (TokenSpawn tspawn in m_TokenSpawns)
        {
            if (tspawn.m_oObject==ttg)
            {
                m_aoPlayerHUDs[0].SetToken(tspawn.m_tType);
                tspawn.m_bSpawning = true;
                tspawn.m_fSpawnTime = Time.fixedTime + 5.0f;
                Destroy(ttg.gameObject);

            }
        }
    }
}
