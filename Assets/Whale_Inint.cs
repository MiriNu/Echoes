using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale_Inint : MonoBehaviour {
    public GameObject Whale;
    public float time;
	// Use this for initialization
    float next_spawn_time;
	void Start () {
        next_spawn_time = Time.time + time;
	}
	
	// Update is called once per frame
	void Update () {
        
            
        if (Time.time > next_spawn_time)
        {
            
            Vector3 position = new Vector3(transform.position.x,Random.Range(26.0f, -13.0f),transform.position.z);
            transform.position = position;
            Instantiate(Whale, transform.position, Quaternion.identity);

            //increment next_spawn_time
            next_spawn_time += time;
        }
	}
}
