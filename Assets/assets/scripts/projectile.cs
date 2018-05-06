using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {
    private Rigidbody2D body;
    private Animator selfAni;
    private SpriteRenderer selfSprite;
    private GameObject owner;
    public float damage;
    private Vector2 velocity;
    public float destroyTime = 0;
	// Use this for initialization
	void Start () {
        selfSprite = this.GetComponent<SpriteRenderer>();
        selfAni = this.GetComponent<Animator>();
        body = this.GetComponent<Rigidbody2D>();
        if (destroyTime == 0)
        {
            destroyTime = Time.fixedTime + 5f;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Player" && !other.Equals(owner))
        {
            selfAni.SetBool("collide", true);
            body.velocity = Vector2.zero;
            FindObjectOfType<RoundManager>().DamagePlayer(other, damage);
            destroyTime = Time.fixedTime + 0.25f;
        }
        owner.SendMessage("stopGen", null);
    }

    void setVel(Vector2 vel)
    {
        velocity = vel;
    }

    void setRotation(float rot)
    {
        this.body.rotation = rot;
    }

    void setOwner(GameObject own)
    {
        owner = own;
    }
	
	// Update is called once per frame
	void Update () {
        selfSprite.flipX = (velocity.x < -.01);
        if (Time.fixedTime >= destroyTime)
        {
            Destroy(this.gameObject);
            owner.SendMessage("stopGen", null);
        }

        this.body.AddForce(velocity);
	}
}
