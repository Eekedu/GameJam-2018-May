using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float speed = 500f;
    float timeLast;
    public float jumpingForce = 100f;
    bool canJump = true;
    private Vector2 velocity;
    Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void move(Vector2 dir)
    {
        dir.Scale(new Vector2(speed, speed));
        velocity.x = dir.x;
    }

    void jump()
    {
        if (canJump)
        {
            Vector2 upForce = Vector2.up * jumpingForce;
            velocity.y = upForce.y;
            timeLast = Time.fixedTime + 0.25f;
            Debug.Log(timeLast);
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
        if (Mathf.Abs(velocity.x) > speed)
        {
            velocity.x = speed;
        }
        velocity.x *= 0.7f;
        //Debug.Log(Time.fixedTime + " " + timeLast);
        if (Time.fixedTime >= timeLast)
        {
            timeLast = 0;
            velocity.y = 0;
        }
        body.AddForce(velocity);
        transform.position = new Vector2(transform.position.x + velocity.x, transform.position.y);
    }
}
