using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {
    private Rigidbody2D body;
    private Vector2 velocity;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}

    void setVel(Vector2 vel)
    {
        velocity = vel;
    }
	
	// Update is called once per frame
	void Update () {
        this.body.AddForce(velocity);
	}
}
