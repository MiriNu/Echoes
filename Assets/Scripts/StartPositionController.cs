using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPositionController : MonoBehaviour {

    private Vector3 start;

	// Use this for initialization
	void Start () {
        start = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = start;
	}
}
