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
    public GameManager prefTestManager;

    private int m_iPlayerCount;
    private PlayerHUDScript[] m_aoPlayerHUDs;
    private CameraScript m_oGameCamera;
    private ActivePlayer[] m_oActivePlayers;
    private int m_iPlayersRemaining;

    public ActivePlayer[] GetPlayers()
    {
        return this.m_oActivePlayers;
    }


    private class TokenSpawn
    {
        public Vector2 m_vPosition;
        public TokenScript.TokenType m_tType;
        public TokenScript m_oObject;
        public bool m_bSpawning;
        public float m_fSpawnTime;
    }

    public class ActivePlayer
    {
        public GameObject m_oObject;
        public playerEffector m_oEffector;
        public playerController m_oController;
        public PlayerHUDScript m_oHUD;
        public float m_fHealth;
        public bool m_bAlive;
        public bool m_bInGame;
        public bool m_bSpawning;
        public float m_fSpawnTime;
        public int m_iStocks = 2;
        public int m_iArrayIndex;
        public int m_iControllerOrdinal;
        public ActivePlayer()
        {
            m_bAlive = false;
            m_bSpawning = false;
            m_bInGame = true;
        }
        public Vector2 GetPosition()
        {
            return new Vector2(m_oObject.transform.position.x, m_oObject.transform.position.y);
        }
        public void AssignPlayerObject(GameObject playob)
        {
            m_oObject = playob;
            m_oController = playob.GetComponent<playerController>();
            m_oEffector = playob.GetComponent<playerEffector>();
        }
        public void AssignArrayNumber(int dex)
        {
            m_iArrayIndex = dex;
        }
        public void AssignControllerOrdinal(int iOrd, PlayerHUDScript nuHUD)
        {
            m_iControllerOrdinal = iOrd;
            m_oHUD = nuHUD;
            m_oHUD.SetPlayerTitle("Player " + iOrd.ToString());
        }
    }

    private enum Phase
    {
        RP_Initial,
        RP_Starting,
        RP_Playing,
        RP_Ending,
        RP_Leaving
    }

    private Phase m_pPhase = Phase.RP_Initial;
    private float m_fStartTime;
    private Text m_tOverlayText;
    private GameManager m_oGameManager;

	// Use this for initialization
	void Start () {
        m_oGameManager = FindObjectOfType<GameManager>();
        if (m_oGameManager == null) m_oGameManager = Instantiate(prefTestManager);
            SetPlayerCount(m_oGameManager.GetControllerCount());
        m_tOverlayText = GetComponentInChildren<Text>();
        if (SceneBoss.g_oSceneBoss!=null) SceneBoss.g_oSceneBoss.FadeIn();
        ConvertPlayerSpawns();
        ConvertTokenSpawns();
        EnterPhaseStart();
        m_oGameCamera = GetComponentInChildren<CameraScript>();
        


    }

    public void SetPlayerCount(int count)
    {
        float fXStart = Screen.width * -0.5f;
        float fYStart = (GetComponent<Canvas>().pixelRect.height * -0.45f);// (Screen.height * - 0.25f) + 000;
        m_iPlayerCount = count;
        m_aoPlayerHUDs = new PlayerHUDScript[count];
        float fHUDStep = Screen.width / (count + 1);
        Debug.Log("<" + fXStart.ToString() + "> <" + fYStart.ToString() + "> <" + fHUDStep.ToString() + ">");
        m_oActivePlayers = new ActivePlayer[count];
        for (int i = 0; i < count; i++)
        {
            float fXPos = fXStart + ((i+1) * (fHUDStep));
            //m_aoPlayerHUDs[i] = Instantiate(m_preHUD, new Vector3(fPos, 55), Quaternion.identity,this.transform);
            m_aoPlayerHUDs[i] = Instantiate(m_preHUD, this.transform);
            m_aoPlayerHUDs[i].transform.localPosition = new Vector3(fXPos, fYStart);
            //m_aoPlayerHUDs[i].SetStocks(2);
            m_oActivePlayers[i] = new ActivePlayer();
            m_oActivePlayers[i].m_oHUD = m_aoPlayerHUDs[i];
            m_oActivePlayers[i].AssignArrayNumber(i);
            m_oActivePlayers[i].AssignControllerOrdinal(m_oGameManager.GetControllerIndex(i),m_aoPlayerHUDs[i] );
        }
        m_iPlayersRemaining = count;
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
        foreach (ActivePlayer aplay in m_oActivePlayers)
        {
            SpawnPlayer(aplay.m_iArrayIndex);
        }
        m_fStartTime = Time.fixedTime +5.0f;
    }
    private void ProcPhasePlay()
    {
        SpawnTokens();
        if (m_fStartTime >= Time.fixedTime)
        {
            m_tOverlayText.enabled = false;
        }
        SpawnPlayers();
    }

    private void EnterPhaseEnd()
    {
        m_pPhase = Phase.RP_Ending;
        foreach (ActivePlayer aplay in m_oActivePlayers)
        {
            aplay.m_oController.Freeze();
            Debug.Log("WinnerWinner");
            if (aplay.m_bInGame)
            {
                Debug.Log("Chicken Dinner");
                m_oGameManager.SetWinner(aplay.m_iControllerOrdinal);
            }
        }
        m_fStartTime = Time.fixedTime + 2.0f;
    }
    private void ProcPhaseEnd()
    {
        if (Time.fixedTime>=m_fStartTime)
        {
            if (SceneBoss.g_oSceneBoss!=null) SceneBoss.g_oSceneBoss.FadeOut();
            EnterPhaseLeave();
        }

    }

    private void EnterPhaseLeave()
    {
        m_pPhase = Phase.RP_Leaving;
    }

    private void ProcPhaseLeave()
    {
        if (SceneBoss.g_oSceneBoss != null)
        {
            if (SceneBoss.g_oSceneBoss.FadeComplete()) {
                UnityEngine.SceneManagement.SceneManager.LoadScene(SceneBoss.g_oSceneBoss.GetSceneIndex(SceneBoss.SceneSelect.SS_GameOver));
            }
        } else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
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
        m_oActivePlayers[playerdex].AssignPlayerObject(Instantiate(playerPrefab, new Vector3(m_v2PlayerSpawns[playerdex].x, m_v2PlayerSpawns[playerdex].y), Quaternion.identity));
        m_oActivePlayers[playerdex].m_fHealth = 100;
        m_oActivePlayers[playerdex].m_oHUD.SetHealth(100, false);
        m_oActivePlayers[playerdex].m_oHUD.SetStocks(m_oActivePlayers[playerdex].m_iStocks);
        m_oActivePlayers[playerdex].m_bSpawning = false;
        m_oActivePlayers[playerdex].m_bAlive = true;
        m_oActivePlayers[playerdex].m_oObject.GetComponent<playerController>().AssignControllerNumber(m_oActivePlayers[playerdex].m_iControllerOrdinal);
    }

    private void KillPlayer(GameObject player)
    {
        foreach (ActivePlayer play in m_oActivePlayers)
        {
            if (play.m_oObject == player)
            {
                play.m_bAlive=false;
                play.m_iStocks -= 1;
                play.m_oHUD.SetStocks(play.m_iStocks);
                play.m_bInGame = play.m_iStocks > 0;
                play.m_oController.Freeze();
                Destroy(play.m_oObject);
                if (play.m_bInGame == false)
                {
                    play.m_oHUD.SetPlayerTitle("Fucking dead");
                    m_iPlayersRemaining -= 1;
                    if (m_iPlayersRemaining <= 1)
                    {
                        EnterPhaseEnd();
                    }
                }
                else {
                    play.m_bSpawning = true;
                    play.m_fSpawnTime = Time.fixedTime + 3.0f;
                }
            }
        }
    }

    public void DamagePlayer(GameObject player, float fDamage)

    {
        foreach (ActivePlayer play in m_oActivePlayers) {
            if (play.m_oObject == player)
            {
                play.m_fHealth -= fDamage;
                play.m_oHUD.SetHealth(play.m_fHealth, true);
                if (play.m_fHealth <= 0 && play.m_bAlive)
                {
                    KillPlayer(player);
                }
            }
        }
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
    private void SpawnPlayers()
    {

        foreach (ActivePlayer tplay in m_oActivePlayers)
        {
            if (tplay.m_bSpawning)
            {
                if (Time.fixedTime >= tplay.m_fSpawnTime)
                {
                    SpawnPlayer(tplay.m_iArrayIndex);
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
            if (player == null) continue;
            if (!player.m_bAlive) continue;
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
            case Phase.RP_Ending:
                ProcPhaseEnd();
                break;
            case Phase.RP_Leaving:
                ProcPhaseLeave();
                break;
        }

        UpdatePlayerBounds();
        m_oGameCamera.SetBounds(m_vTopLeftBound, m_vBottomRightBound);
        //SpawnTokens();
    }
    public void GrabToken(TokenScript ttg, GameObject player)
    {
        foreach (TokenSpawn tspawn in m_TokenSpawns)
        {
            if (tspawn.m_oObject==ttg)
            {
                //m_aoPlayerHUDs[0].SetToken(tspawn.m_tType);
                foreach (ActivePlayer play in m_oActivePlayers)
                {
                    if (player==play.m_oObject)
                    {
                        play.m_oHUD.SetToken(tspawn.m_tType);
                    }
                }
                tspawn.m_bSpawning = true;
                tspawn.m_fSpawnTime = Time.fixedTime + 5.0f;
                Destroy(ttg.gameObject);

            }
        }
    }
}
