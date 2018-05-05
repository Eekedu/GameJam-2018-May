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
        Debug.Log(playerPrefix + "Horizontal is " + moveX.ToString() );
        moveX = Mathf.Clamp(moveX, -1.0f, 1.0f);
        //SendMessage("move", new Vector2(moveX, 0f), SendMessageOptions.RequireReceiver);
        mmove.move(new Vector2(moveX, 0.0f));
        if (Mathf.Abs(moveX) >= 0.01f)
        {
            //            SendMessage("move", new Vector2(moveX, 0f), SendMessageOptions.RequireReceiver);
        }
        else
        {
            mmove.stop();
        }
    }

    // Update is called once per frame
    void Update () {


        if (Input.GetKey(KeyCode.A))
        {
            //this.gameObject.SendMessage("move", new Vector2(-1.0f, 0f));
        }
        if (Input.GetKey(KeyCode.D))
        {
            //this.gameObject.SendMessage("move", new Vector2(1.0f, 0f));
        }
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
