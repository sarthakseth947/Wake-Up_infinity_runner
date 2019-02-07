using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    Transform lookAt;
    Vector3 startoffset;
    Vector3 moveVector;
	GameObject player;
	// Use this for initialization
	void Start () {
        lookAt=GameObject.FindGameObjectWithTag("Player").transform;
        startoffset = transform.position - lookAt.position;
		player = GameObject.FindGameObjectsWithTag ("Player")[0];
	}
	
	// Update is called once per frame
	void Update () {
		bool reachedEnd = player.GetComponent<PlayerMotor> ().reachedEnd;
		if (!reachedEnd) {
			moveVector = lookAt.position + startoffset;
			//moveVector.x = 0;
			moveVector.y = Mathf.Clamp (moveVector.y, lookAt.position.y, lookAt.position.y + 5.5f);
			transform.position = moveVector;
		} else {
			transform.LookAt (player.transform);
			Vector3 turnSpeed = new Vector3 (2, 0, 0);
			transform.Translate (turnSpeed * Time.deltaTime);
		}
	}
}
