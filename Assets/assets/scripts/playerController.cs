using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
    public string playerPrefix;
    GameObject pickup;
    movement mmove;
	// Use this for initialization
	void Start () {
        mmove = GetComponent<movement>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pickup = collision.gameObject;
    }

    public void AssignPlayerNumber(int dex)
    {
        playerPrefix = "P" + dex.ToString() + "_";
    }

    private void FixedUpdate()
    {
        float moveX = (Input.GetAxis(playerPrefix + "Horizontal"));
        moveX = Mathf.Clamp(moveX, -1.0f, 1.0f);
        mmove.move(new Vector2(moveX, 0.0f));
        if (Mathf.Abs(moveX) <= 0.01f) { 
            mmove.stop();
        }
    }

    // Update is called once per frame
    void Update () {



        if (Input.GetKeyDown(KeyCode.F))
        {
            this.gameObject.SendMessage("pickup",1);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.gameObject.SendMessage("attack", new Vector2(0,0));
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown(playerPrefix + "Fire1"))
        {
            Debug.Log(playerPrefix + "Fire1");
            SendMessage("jump", null, SendMessageOptions.RequireReceiver);
        }
        if (Input.GetButtonDown(playerPrefix + "Fire2"))
        {
            SendMessage("pickup", 1, SendMessageOptions.RequireReceiver);
        }
	}
}
