using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anurexia : Enemy
{
    private Transform Anurex;
    private Transform FirePos;
    private bool faceLeft = true;
    public bool spottedPlayers = false;
    private Rigidbody2D RiGAnurexia;
    public Vector2 speed;
    public GameObject Player;
    public GameObject RightAnorexBullet;
    public GameObject LeftAnorexBullet;
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
    // Use this for initialization
    void Start()
    {
        RiGAnurexia = GetComponent<Rigidbody2D>();
        Anurex = GetComponent<Transform>();
        RiGAnurexia.constraints = RigidbodyConstraints2D.FreezeRotation;
        FirePos = GetComponent<Transform>().Find("FirePos"); 
        Player = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            if (Anurex.position.x > Player.transform.position.x && spottedPlayers)
            {
                
                RiGAnurexia.velocity = -speed;
                if (!faceLeft)
                {
                    Anurex.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                    faceLeft = true;

                }
            }
            if (Anurex.position.x < Player.transform.position.x && spottedPlayers)
            {
                
                RiGAnurexia.velocity = speed;
                if (faceLeft)
                {
                    Anurex.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                    faceLeft = false;
                }
            }


        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }


        if (!faceLeft && spottedPlayers && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(LeftAnorexBullet, FirePos.position, Quaternion.identity);
        }
        if (faceLeft && spottedPlayers && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(RightAnorexBullet, FirePos.position, Quaternion.identity);
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            spottedPlayers = true;
            Player = other.gameObject;
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            spottedPlayers = false;
            Player = null;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            health -= playerDamage;
        }
    }
}