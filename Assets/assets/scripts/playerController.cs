using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
    public string playerPrefix;
    GameObject pickup;
	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pickup = collision.gameObject;
    }

    // Update is called once per frame
    void Update () {
        float moveX = (Input.GetAxis(playerPrefix + "Horizontal"));
        if (moveX != 0)
        {
            SendMessage("move", new Vector2(moveX, 0f), SendMessageOptions.RequireReceiver);
        }

        if (Input.GetButtonDown(playerPrefix + "Fire1"))
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
