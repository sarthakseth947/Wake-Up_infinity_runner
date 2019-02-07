using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {

	public GameObject camera;
	public GameObject floorParent;
	public float spawnLength;
	Queue<Object> Q = new Queue<Object>();

	// Use this for initialization
	void Start () {
		int i;
		float diff = (transform.position.z - camera.transform.position.z);
		while (diff < spawnLength) {
			SpawnZ ();
			diff = (transform.position.z - camera.transform.position.z);
		}
	}

	// Update is called once per frame
	void Update () {
		float diff = transform.position.z - camera.transform.position.z;
		if(diff < spawnLength){
			SpawnZ ();
		}
		DeleteZ ();
	}

	void SpawnZ(){
		Vector3 pos = transform.position;
		int op = Random.Range (0, floorParent.transform.childCount);
		//Debug.Log ("Spawning " + (op+1).ToString()+ " " + pos.ToString());
		Q.Enqueue (Instantiate (floorParent.transform.GetChild (op).gameObject, pos, Quaternion.identity));
		Vector3 end = floorParent.transform.GetChild (op).Find ("end").position;
		//Debug.Log ("End at: " + end.ToString ());
		pos += end;
		transform.position = pos;
		//Debug.Log ("PlatformSpawner moved to: "+pos.ToString());

	}
	void DeleteZ(){
		//Deletion
		GameObject tmp = (GameObject) Q.Peek();
		Vector3 qfrontEnd = tmp.transform.Find("end").position;
		Vector3 cameraPos = camera.transform.position;
		float diff = cameraPos.z - qfrontEnd.z;
		if (diff > 0) {
			//Debug.Log ("Deleting " + tmp.name);
			Destroy (tmp);
			Q.Dequeue ();
		}
	}
}
