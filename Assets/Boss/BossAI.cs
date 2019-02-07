using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {
    public GameObject player;
    Animator anim;
    public GameObject GetPlayer() {
        return player;
    }
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Attack");
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Attacking");
        }
    }

    // Update is called once per frame
    void Update () {
        Vector3 direction = (player.transform.position - this.transform.position);
        anim.SetFloat("distance", direction.magnitude);
        
	}
}
