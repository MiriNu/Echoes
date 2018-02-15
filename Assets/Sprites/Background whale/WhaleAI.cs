using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WhaleAI : MonoBehaviour {
    private Rigidbody2D whaleBody;
    private bool faceRight = false;
    public Vector2 speed;
    public AudioClip whaleMusic;
    private AudioSource aud;
    // Use this for initialization
    void Start()
    {
        whaleBody = GetComponent<Rigidbody2D>();
        whaleBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        aud = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        whaleBody.velocity = speed;

        if (gameObject.transform.position.x < -15.83)
            Destroy(gameObject);
        
        aud.PlayOneShot(whaleMusic, 0.06f);
        whaleMusic = null;
	}
}
