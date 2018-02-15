using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AnorexiaBullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D AnorBullet;
    private AnorexiaAI anorexiaAI;
    private GameObject[] anorexias;
    private GameObject closestAnorexia;
    private float minDist = Mathf.Infinity;
    //public AudioClip shot;
    //AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        AnorBullet = GetComponent<Rigidbody2D>();
        if (anorexias == null)
        {
            anorexias = GameObject.FindGameObjectsWithTag("AnorexiaFirePos");
        }
        foreach (GameObject fp in anorexias)
        {
            float dist = Vector3.Distance(fp.transform.position, transform.position);
            if (dist < minDist)
            {
                closestAnorexia = fp;
                minDist = dist;
            }
        }
        anorexiaAI = closestAnorexia.GetComponentInParent<AnorexiaAI>();
        if (anorexiaAI.target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            AnorBullet.velocity = (anorexiaAI.target.transform.position - transform.position) * speed;
        }
        //audioSource = GetComponent<AudioSource>();
        //audioSource.PlayOneShot(shot, 0.7F);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
        else
            Destroy(gameObject);
    }
}

