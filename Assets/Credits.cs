using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour {
    public AudioSource ClickSound;
    public AudioClip buttonSound;
	// Use this for initialization
	void Start () {
        ClickSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnBackGameButtonClicked()
    {
        ClickSound.PlayOneShot(buttonSound);
        //MyLvlManager.S.LoadLevel("Demo");
        Initiate.Fade("main menu", Color.black, 1f);
    }
}
