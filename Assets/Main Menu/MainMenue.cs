using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenue : MonoBehaviour {
    public Texture background;
    public float GUIplacementY,GUIplacementX;
    public AudioSource ClickSound;
    public AudioClip buttonSound;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartGameButtonClicked()
    {
        ClickSound.PlayOneShot(buttonSound);
        //MyLvlManager.S.LoadLevel("Demo");
        Initiate.Fade("Demo",Color.black,1f);
    }
    public void OnExitGameButtonClicked()
    {
        ClickSound.PlayOneShot(buttonSound);
        Application.Quit();
    }
    public void OnStartOverButtonClicked()
    {
        ClickSound.PlayOneShot(buttonSound);
        //MyLvlManager.S.LoadLevel("Demo");
        Initiate.Fade("Demo", Color.black, 2f);
    }
    public void OnMainMenueGameButtonClicked()
    {
        ClickSound.PlayOneShot(buttonSound);
        //MyLvlManager.S.LoadLevel("main menu");
        Initiate.Fade("main menu", Color.black, 2f);
    }
    public void OnCreditsButtonClicked()
    {
        ClickSound.PlayOneShot(buttonSound);
        Initiate.Fade("Creators", Color.black,0.5f);
    }

}
