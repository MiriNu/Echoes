using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public AudioClip shieldSound;
    AudioSource audioSource;
    private BoxCollider2D box;
    private Rigidbody2D boxRig;
    public float delay;
	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider2D>();
        boxRig = GetComponent<Rigidbody2D>();
        boxRig.constraints = RigidbodyConstraints2D.FreezeRotation;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(shieldSound, 0.7F);
        //Destroy(gameObject, delay);

	}
	

	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "AnorexBullet" || collision.gameObject.tag == "EnemyBullet")
            Destroy(gameObject);
    }
}
