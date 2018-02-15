using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knockBackBulletControl : MonoBehaviour
{
    private Rigidbody2D bulletRB;

    public float speed = 5.0f;
    public float pushStrength = 5.0f;

    // Use this for initialization
    void Start ()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        bulletRB.velocity = Vector2.right * speed;
	}

    // Update is called once per frame
    void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(bulletRB.velocity * pushStrength);
        }
        Destroy(gameObject);
    }
}
