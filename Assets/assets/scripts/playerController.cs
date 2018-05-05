using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
    public string playerPrefix = "P1_";
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
            this.gameObject.SendMessage("move", new Vector2(moveX, 0f));
        }

        if (Input.GetButtonDown(playerPrefix + "Fire1"))
        {
            this.gameObject.SendMessage("jump", null);
        }
        if (Input.GetButtonDown(playerPrefix + "Fire2"))
        {
            Debug.Log("pickup");
            this.gameObject.SendMessage("pickup", 1);
        }
	}
}
