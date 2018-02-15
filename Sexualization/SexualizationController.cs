using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SexualizationController : MonoBehaviour
{
    private GameObject sexualization;
    private Rigidbody2D sexRB;
    private bool playerLock = false;

    public Transform firePos;
    public bool facingRight = true;
    public float speed = 1.5f;
    public float fireRate = 0.333f;
    public float nextFire = 0.0f;
    public GameObject sexBulletRight;
    public GameObject sexBulletLeft;

    // Use this for initialization
    void Start()
    {
        sexualization = GetComponent<GameObject>();
        sexRB = GetComponent<Rigidbody2D>();
        sexRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(facingRight && transform.position.x > collision.transform.position.x && !playerLock)
            {
                transform.localScale = new Vector2(transform.localScale.x * (-1), transform.localScale.y);
                facingRight = false;
            }
            else if(!facingRight && transform.position.x < collision.transform.position.x && !playerLock)
            {
                transform.localScale = new Vector2(transform.localScale.x * (-1), transform.localScale.y);
                facingRight = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerLock = true;
            if (facingRight)
            {
                sexRB.velocity = Vector2.right * speed;
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    Instantiate(sexBulletRight, firePos.position, Quaternion.identity);
                }
            }
            else
            {
                sexRB.velocity = Vector2.left * speed;
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    Instantiate(sexBulletLeft, firePos.position, Quaternion.identity);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerLock = false;
            sexRB.velocity = new Vector2(0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerBullet")
        {
            Destroy(gameObject);
        }
    }
}