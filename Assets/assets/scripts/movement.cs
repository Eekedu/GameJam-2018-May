using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float speed = 5f;
    public float hMoveForce = 500f;
    private float timeLast;
    public float jumpingForce = 100f;
    public RuntimeAnimatorController airC, fireC, elecC, earthC, waterC;
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

    public void move(Vector2 dir)
    {
            float hComponent = dir.x;
            body.AddForce(Vector2.right * hComponent * hMoveForce);
            body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -speed, speed), body.velocity.y);
            selfSpri.flipX = (body.velocity.x > .1);
            selfAni.SetBool("run", true);
            this.isRunning = true;
    }

    public void setAnim(TokenScript.TokenType type)
    {
        RuntimeAnimatorController change = null;
        switch (type)
        {
            case TokenScript.TokenType.TokenFire: change = fireC; break;
            case TokenScript.TokenType.TokenWind: change = airC; break;
            case TokenScript.TokenType.TokenElectric: change = elecC; break;
            case TokenScript.TokenType.TokenEarth: change = earthC; break;
            case TokenScript.TokenType.TokenWater: change = waterC; break;
        }
        Debug.Log(change);
        //AnimatorOverrideController overide = new AnimatorOverrideController((RuntimeAnimatorController)Resources.Load("assets/anims/playerAir"));
        if (change != null)
        {
            selfAni.runtimeAnimatorController = change as RuntimeAnimatorController;
        }
    }

    public void stop()
    {
        body.velocity = new Vector2(0, body.velocity.y) ;
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
        selfAni.SetBool("isFalling", body.velocity.y < -0.0001f);
        selfAni.SetBool("isJumping", (body.velocity.y > 0.1f));
        //body.AddForce(velocity);
        //transform.position = new Vector2(transform.position.x + velocity.x, transform.position.y);
    }
}
