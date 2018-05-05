using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
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
        float moveX = (Input.GetAxis("Horizontal"));
        if (moveX != 0)
        {
            SendMessage("move", new Vector2(moveX, 0f));
        }

        if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire1"))
        {
            SendMessage("jump", null);
        }
        if (Input.GetKeyDown("f") || Input.GetButtonDown("Fire2"))
        {
            Debug.Log("pickup");
            SendMessage("pickup", 1);
        }
	}
}
