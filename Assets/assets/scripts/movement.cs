using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float speed = 500f;
    private float timeLast;
    public float jumpingForce = 100f;
    private bool canJump = true;
    private bool isRunning = false;
    private Vector2 velocity;
    private SpriteRenderer selfSpri;
    private Rigidbody2D body;
    private Animator selfAni;

    private void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
        selfAni = this.GetComponent<Animator>();
        selfSpri = this.GetComponent<SpriteRenderer>();
    }

    void move(Vector2 dir)
    {
        if (!this.isRunning)
        {
            dir.Scale(new Vector2(speed, speed));
            velocity.x = dir.x;
            selfSpri.flipX = (velocity.x > .1);
            selfAni.SetBool("run", true);
            Debug.Log("B");
            this.isRunning = true;
        }
    }

    void stop()
    {
        velocity.x = 0;
        selfAni.SetBool("run", false);
        this.isRunning = false;
    }

    void jump()
    {
        if (canJump)
        {
            Vector2 upForce = Vector2.up * jumpingForce;
            velocity.y = upForce.y;
            timeLast = Time.fixedTime + 0.25f;
            canJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            canJump = true;
        }
    }

    private void Update()   
    {
        velocity.x *= 2f;
        if (Mathf.Abs(velocity.x) > 2f)
        {
            velocity.x = (selfSpri.flipX)?2f:-2f;
        }
        if (Time.fixedTime >= timeLast)
        {
            timeLast = 0;
            velocity.y = 0;
        } else
        {
            velocity.y *= 0.95f;
        }
        selfAni.SetBool("isJumping", velocity.y > 0);
        body.AddForce(velocity);
        transform.position = new Vector2(transform.position.x + velocity.x, transform.position.y);
    }
}
