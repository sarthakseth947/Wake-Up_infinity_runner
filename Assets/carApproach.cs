using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carApproach : MonoBehaviour {

	bool activated=false;
	bool finished=false;
	private GameObject player;
	public float distance;
	public float speed;
	private float initz;
	Collider collider;

	// Use this for initialization
	void Start () {
		player = (GameObject) GameObject.FindGameObjectWithTag ("Player");
		initz = transform.position.z;
		collider = GetComponent<MeshCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!activated) {
			float playerz = player.transform.position.z;
			float carz = transform.position.z;
			if (playerz < carz && carz - playerz < distance) {
				activated = true;
			}
		}
		if (activated && !finished) {
			transform.Translate(Vector3.back * -speed * Time.deltaTime);
		}

		if (!finished) {
			float curz = transform.position.z;
			if (initz - curz >= 30)
				finished = true;
		}
	}

	void onCollisionEnter(Collision col){
		finished = true;
	}

}
