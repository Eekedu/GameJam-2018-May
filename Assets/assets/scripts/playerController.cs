﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
    public string playerPrefix="P1_";
    GameObject pickup;
    movement mmove;
	// Use this for initialization
	void Start () {
        mmove = GetComponent<movement>();
        //AssignControllerNumber(3);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pickup = collision.gameObject;
    }

    public void AssignControllerNumber(int dex)
    {
        playerPrefix = "P" + dex.ToString() + "_";
    }

    private void FixedUpdate()
    {
        if (m_bFrozen) return;
        float moveX = (Input.GetAxis(playerPrefix + "Horizontal"));
        moveX = Mathf.Clamp(moveX, -1.0f, 1.0f);
        mmove.move(new Vector2(moveX, 0.0f));
        if (Mathf.Abs(moveX) <= 0.01f) { 
            mmove.stop();
        }
    }

    bool m_bFrozen;
    public void Freeze()
    {
        m_bFrozen = true;
    }
    public void UnFreeze()
    {
        m_bFrozen = false;
    }
     
    // Update is called once per frame
    void Update () {

        if (m_bFrozen) return;



        if (Input.GetButtonDown(playerPrefix + "Fire"))
        {
            Vector2 dir = (mmove.selfSpri.flipX)?Vector2.right:Vector2.left;
            this.gameObject.SendMessage("attack", dir);
        }

        if (Input.GetButton(playerPrefix + "Jump"))
        {
            SendMessage("jump", null, SendMessageOptions.RequireReceiver);
        }
        if (Input.GetButtonDown(playerPrefix + "Grab"))
        {
            SendMessage("pickup", 1, SendMessageOptions.RequireReceiver);
        }
	}
}
