using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnorexBull : MonoBehaviour {
    public Vector2 speed;
    private Rigidbody2D AnorexBullet;
    //public AudioClip shot;
    //AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        AnorexBullet = GetComponent<Rigidbody2D>();
        AnorexBullet.velocity = speed;
        //audioSource = GetComponent<AudioSource>();
        //audioSource.PlayOneShot(shot, 0.7F);
    }

    // Update is called once per frame
    void Update()
    {
        AnorexBullet.velocity = speed;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
