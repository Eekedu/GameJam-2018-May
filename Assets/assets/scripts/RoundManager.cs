using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {

    Vector2[] m_v2PlayerSpawns;
    Vector2[] TokenSpawns;
    public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
        playerController[] pcspots = FindObjectsOfType<playerController>();
        m_v2PlayerSpawns = new Vector2[pcspots.Length];
        int ddex = 0;
        foreach (playerController pcon in pcspots)
        {
            m_v2PlayerSpawns[ddex] = new Vector2(pcon.transform.position.x,pcon.transform.position.y);
            Destroy(pcon.gameObject);
        }
        spawntimetest = Time.fixedTime + 3.0f;
        //TokenSpawns = FindObjectsOfType<TokenScript>();
	}
	
    private void SpawnPlayer(int playerdex)
    {
        Instantiate(playerPrefab, new Vector3(m_v2PlayerSpawns[0].x, m_v2PlayerSpawns[0].y), Quaternion.identity);
    }

    bool spawntest = true;
    float spawntimetest;
	// Update is called once per frame
	void Update () {
        if (spawntest)
        {
            if (Time.fixedTime >= spawntimetest)
            {
                SpawnPlayer(1);
                spawntest = false;
            }
        }
    }
}
