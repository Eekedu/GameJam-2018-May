using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float speed = 5f;
    public float jumpingForce = 1f;
    bool canJump = true;
    private Vector2 velocity;
    private Vector2 velocityDampener = new Vector2(0.7f, 0.7f);

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
            velocity.y += upForce.y;
            if (velocity.y > 5f)
            {
                canJump = false;
            }
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
        Vector2 pos = transform.position;
        if (Mathf.Abs(velocity.x) > speed)
        {
            velocity.x = speed;
        }
        velocity.Scale(velocityDampener);
        transform.position = new Vector2(pos.x + velocity.x, pos.y + velocity.y);
    }
}
