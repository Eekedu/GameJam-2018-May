using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {
    private Rigidbody2D body;
    private Animator selfAni;
    private SpriteRenderer selfSprite;
    private GameObject owner;
    private AudioSource AudSrc;
    public AudioClip hitAudio;
    public float damage;
    private Vector2 velocity;
    private float destroyTime = 0;
    public float genTime = 0f;
    public bool canCollideDestroy = true;
    public bool hasHit = false;
	// Use this for initialization
	void Start () {
        AudSrc = this.GetComponent<AudioSource>();
        if (hitAudio != null)
        {
            AudSrc.clip = hitAudio;
        }
        selfSprite = this.GetComponent<SpriteRenderer>();
        selfAni = this.GetComponent<Animator>();
        body = this.GetComponent<Rigidbody2D>();
        destroyTime = Time.fixedTime + 5f;
        if (this.gameObject.ToString().IndexOf("rockBall") != -1 || this.gameObject.ToString().LastIndexOf("waveAttack") != -1)
        {
            canCollideDestroy = false;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Player" && !other.Equals(owner) && !hasHit)
        {
            selfAni.SetBool("collide", true);
            body.velocity = Vector2.zero;
            RoundManager manager = FindObjectOfType<RoundManager>();
            playerEffector player = other.GetComponent<playerEffector>();
            playerEffector playerMe = owner.GetComponent<playerEffector>();
            TokenScript.TokenType playerType = player.getStatus();
            TokenScript.TokenType ownerType = playerMe.getStatus();

            TokenScript.TokenType[] goodCheck = playerMe.goodAgainst();
            TokenScript.TokenType badCheck = playerMe.badAgainst();
            foreach (TokenScript.TokenType type in goodCheck)
            {
                Debug.Log(type);
                Debug.Log(playerType);
                if (playerType == type)
                {
                    damage += 5f;
                }
            }
            if (badCheck != TokenScript.TokenType.TokenNone)
            {
                if (badCheck == playerType)
                {
                    damage -= 5f;
                    if (damage < 0f)
                    {
                        damage = 0f;
                    }
                }
            }

            manager.DamagePlayer(other, damage);
            destroyTime = Time.fixedTime + 0.25f;
            owner.SendMessage("stopGen", null);
            AudSrc.Play();
            hasHit = true;
        } else if (other.layer == 8 && canCollideDestroy)
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
        selfSprite.flipX = (velocity.x < -.01);
        if (Time.fixedTime >= destroyTime)
        {
            Destroy(this.gameObject);
        }
        if (velocity.y != 0) {
            velocity.y = Mathf.Lerp(velocity.y, 0, 0.075f);
        }
        if (Mathf.Abs(velocity.x) > 40f)
        {
            velocity.x *= 0.9f;
        }
        this.body.AddForce(velocity);
	}
}
