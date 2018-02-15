using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

    public Vector2 speed;
    Rigidbody2D bullet;
    public AudioClip shot;
    AudioSource audioSource;
    private GameObject ghost;
	// Use this for initialization
	void Start () {
        bullet = GetComponent<Rigidbody2D>();
        bullet.velocity = speed;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(shot, 0.7F);
        ghost = GameObject.Find("Ghost");
        Physics2D.IgnoreCollision(ghost.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
	
	
	// Update is called once per frame
	void Update ()
    {
        bullet.velocity = speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Shield")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
        else
             Destroy(gameObject);
    }

}
