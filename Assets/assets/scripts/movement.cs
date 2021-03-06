﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float speed = 10f;
    public float hMoveForce = 300f;
    private float timeLast;
    public float jumpingForce = 10f;
    public RuntimeAnimatorController norm, airC, fireC, elecC, earthC, waterC;
    private bool canJump = true;
    private bool isRunning = false;
    private Vector2 velocity;
    public SpriteRenderer selfSpri;
    private Rigidbody2D body;
    private Animator selfAni;
    private Vector2 windForce;
    private float stopForce = 0f;

    private void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
        selfAni = this.GetComponent<Animator>();
        selfSpri = this.GetComponent<SpriteRenderer>();
    }

    public void addForce(Vector2 dir)
    {
        windForce = dir;
        stopForce = Time.fixedTime + 2f;
    }

    public void move(Vector2 dir)
    {
            float hComponent = dir.x;
        if (Mathf.Abs(body.velocity.x * hMoveForce * Time.fixedDeltaTime) < speed) body.AddForce(Vector2.right * hComponent * hMoveForce);
            body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -speed, speed), body.velocity.y);

//        if ((body.velocity.x > 0.1f) && (!selfSpri.flipX)) selfSpri.flipX = true;
//        if ((body.velocity.x < -0.1f) && (selfSpri.flipX)) selfSpri.flipX = false;
        if ((body.velocity.x > 0.005f)) selfSpri.flipX = true;
        if ((body.velocity.x < -0.005f)) selfSpri.flipX = false;
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
            case TokenScript.TokenType.TokenNone: change = norm; break;
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
        Debug.Log(canJump.ToString());
        if (canJump)
        {
            Debug.Log("Oigo" + jumpingForce.ToString());
            body.AddForce(Vector2.up * jumpingForce);
            canJump = false;
            isJumping = true;
        } else
        {
            if (isJumping)
            {
              body.AddForce(Vector2.up*0.2f);
            }
        }
    }

    bool isJumping;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        return;
        if (collision.gameObject.tag == "Floor")
        {
            canJump = true;
        }
    }

    private void GroundCheck()
    {
        Vector2 vStart = new Vector2(transform.position.x, transform.position.y);
        Vector2 vEnd = vStart - (Vector2.up * 0.3f);
        Debug.DrawLine(new Vector3(vStart.x, vStart.y), new Vector3(vEnd.x, vEnd.y));
        bool grounded =  Physics2D.Linecast(vStart,vEnd,1<<LayerMask.NameToLayer("Ground"));
        canJump = (body.velocity.y <= 0f) && (grounded) && (!isJumping);
    }
    private void Update()   
    {
        if (stopForce != 0 && Time.fixedTime < stopForce)
        {
            body.AddForce(windForce);
        } else
        {
            stopForce = 0f;
        }
        GroundCheck();
        if (body.velocity.y < 0) isJumping = false;
        selfAni.SetBool("isFalling", body.velocity.y < -0.0001f);
        selfAni.SetBool("isJumping", (body.velocity.y > 0.1f));
        //body.AddForce(velocity);
        //transform.position = new Vector2(transform.position.x + velocity.x, transform.position.y);
    }
}
