using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {
    private bool readyToDie;
    public GameObject NastyRummor;
    float next_spawn_time;
	// Use this for initialization
	void Start () {
        next_spawn_time = Time.time + 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
        //if(readyToDie)
        //{
        //    Instantiate(NastyRummor,transform.position,Quaternion.identity);

        //}

        if (Time.time > next_spawn_time && readyToDie)
        {
            //do stuff here (like instantiate)
            Instantiate(NastyRummor, transform.position, Quaternion.identity);

         //increment next_spawn_time
            next_spawn_time += 5.0f;
        }
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            readyToDie = true;
    }
}
