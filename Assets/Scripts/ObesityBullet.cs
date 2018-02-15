using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObesityBullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D ObesBullet;
    private ObesityAI obesityAI;
    private GameObject[] obesities;
    private GameObject closestObesity;
    private float minDist = Mathf.Infinity;
    //public AudioClip shot;
    //AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        ObesBullet = GetComponent<Rigidbody2D>();
        if (obesities == null)
        {
            obesities = GameObject.FindGameObjectsWithTag("ObesityFirePos");
        }
        foreach (GameObject fp in obesities)
        {
            float dist = Vector3.Distance(fp.transform.position, transform.position);
            if (dist < minDist)
            {
                closestObesity = fp;
                minDist = dist;
            }
        }
        obesityAI = closestObesity.GetComponentInParent<ObesityAI>();
        if (obesityAI.target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            ObesBullet.velocity = (obesityAI.target.transform.position - transform.position) * speed;
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
