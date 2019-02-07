using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Boss_movements : MonoBehaviour
{
    public GameObject goal;
    public float accuracy;
    public float speed;
    public Slider HealthBar;
    Animator anim;
    AudioSource audioSource;
    public AudioClip[] audioClip;
    public bool bossMoveFlag = true;
    public void setMoveBool(bool val) {
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking1", false);
        anim.SetBool("isAttacking2", false);
        bossMoveFlag = false;
        Debug.Log("JUST SET FALSE");
    }
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playAudio(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (bossMoveFlag)
        {
            this.transform.LookAt(goal.transform.position);
            Vector3 direction = goal.transform.position - this.transform.position;
            if (direction.magnitude > accuracy)
            {
                this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
                anim.SetBool("isRunning", true);
                anim.SetBool("isAttacking1", false);
                anim.SetBool("isAttacking2", false);
               
            }
            else
            {
                anim.SetBool("isRunning", false);
                int number=Random.Range(1,3);
                anim.SetBool("isAttacking"+number, true);

            }
        }
    }
    void playAudio(int clip)
    {
        audioSource.clip = audioClip[clip];
        audioSource.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            HealthBar.value -= 20;
            Debug.Log("Bullet Hit");
            playAudio(2);
            Destroy(other.gameObject, 0f);
        }

        if (HealthBar.value <= 0)
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isAttacking1", false);
            anim.SetBool("isAttacking2", false);
            anim.SetBool("isDead", true);
            StartCoroutine(killPlayer(3));
        }

    }
    IEnumerator killPlayer(float seconds)
    {
        float currentTime = 0;
        bossMoveFlag = false;
        goal.GetComponent<Player_bossLevel_movements>().setMoveBool(false);
        playAudio(3);
        while (currentTime < seconds)
        {
            Debug.Log("CO ROUTINE LOOP");
            currentTime += Time.deltaTime;
            yield return null;
        }
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene + 1, LoadSceneMode.Single);

    }
}
