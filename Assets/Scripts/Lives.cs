using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Lives : MonoBehaviour {

    private AudioSource aud;
    public AudioClip Sound;
    void Start()
    {
        aud = GetComponent<AudioSource>();

    }
	
	
	void Update () {
        
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            aud.PlayOneShot(Sound, 3f);
            Destroy(gameObject,0.2f);
        }
    }

}
