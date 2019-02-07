using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class amovestrip : MonoBehaviour {
    public int speed;
    public float timeline;
    public GameObject parti;
    public int distance;
    public LayerMask whatissolid;
    private int x = 0;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(Des());

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            print("help");
            Instantiate(parti, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "fire"){
            Destroy(gameObject);
        }
    }


    void Update()
    {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    private IEnumerator Des()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            Instantiate(parti, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


}


