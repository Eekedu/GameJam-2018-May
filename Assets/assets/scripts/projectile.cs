using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {
    private Rigidbody2D body;
    private Animator selfAni;
    private GameObject owner;
    private Vector2 velocity;
    private float destroyTime;
	// Use this for initialization
	void Start () {
        selfAni = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        destroyTime = Time.fixedTime + 5f;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Player" && !other.Equals(owner))
        {
            selfAni.SetBool("collide", true);
            body.velocity = Vector2.zero;
            destroyTime = Time.fixedTime + 0.25f;
        }
    }

    void setVel(Vector2 vel)
    {
        velocity = vel;
    }

    void setOwner(GameObject own)
    {
        owner = own;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.fixedTime >= destroyTime)
        {
            Destroy(this.gameObject);
        }
        this.body.AddForce(velocity);
	}
}
