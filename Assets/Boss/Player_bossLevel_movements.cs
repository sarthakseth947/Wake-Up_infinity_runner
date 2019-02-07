using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player_bossLevel_movements : MonoBehaviour {
    public GameObject boss;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    Vector3 moveDirection;
    public float gravity = 20.0f;
    protected Joystick joystick;
    AudioSource audioSource;
    public AudioClip[] audioClip;



    Rigidbody rb;
    CharacterController characterController;
    public Slider HealthBar;
    public float speed = 10.0F;
    public float rotationSpeed = 500.0F;
    private Animator anim;
    private bool playerMoveFlag = true;
    float distToGround;
    //static CapsuleCollider collider;
   

    public void setMoveBool(bool val) {
       
        playerMoveFlag = val;
    }
   //bool IsGrounded() {
   //     return Physics.CheckCapsule(collider.bounds.center, new Vector3(collider.bounds.center.x, collider.bounds.min.y - 0.1f, collider.bounds.center.z), 0.18f);
   // }


private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HealthBar.value -= 1;
            Debug.Log("HitSmall");
            playAudio(1);
        }
        if (other.gameObject.tag == "Enemy2")
        {
            HealthBar.value -= 5;
            Debug.Log("HitBIG");
            playAudio(1);
        }
        
        if (HealthBar.value <= 0) {
            anim.SetBool("isDead", true);
            StartCoroutine(killPlayer(3));
        }

        if (other.gameObject.tag == "Floor")
        {
            Debug.Log("Grounded");
        }
     


    }
    // Use this for initialization
    void Start () {
       // collider = GetComponent<CapsuleCollider>();
       // distToGround = collider.bounds.extents.y;
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        joystick = FindObjectOfType<Joystick>();
        audioSource=GetComponent<AudioSource>();

    }
    void playAudio(int clip)
    {
        audioSource.clip = audioClip[clip];
        audioSource.Play();
    }


    void FixedUpdate()
    {
        if (playerMoveFlag)
        {
            moveDirection = new Vector3(0f, 0.0f, (Input.GetAxis("Vertical") + joystick.Vertical) * Time.deltaTime);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;


            float rotation = (Input.GetAxis("Horizontal") + joystick.Horizontal) * rotationSpeed;
            rotation *= Time.deltaTime;


            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }


            transform.Rotate(0, rotation, 0);

            if (Input.GetKey(KeyCode.UpArrow) || joystick.Vertical > 0.5f)
            {
                anim.ResetTrigger("idle");
                anim.SetTrigger("Run");

                
            }
            
            else if (Input.GetKey(KeyCode.DownArrow) || joystick.Vertical< -0.1f)
            {
                anim.ResetTrigger("idle");
                anim.SetTrigger("BackRun");

            }
            else
            {
                anim.ResetTrigger("Run");
                anim.ResetTrigger("BackRun");
                anim.SetTrigger("idle");

            }


        }

        if (transform.position.y < -5f) {
            anim.ResetTrigger("Run");
            anim.ResetTrigger("idle");
            anim.ResetTrigger("BackRun");
            anim.SetTrigger("Fall");
            StartCoroutine(killPlayerFall(2f));
        }
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        characterController.Move(moveDirection);


    }
    IEnumerator killPlayer(float seconds) {
        float currentTime = 0;
        playAudio(2);
        playerMoveFlag = false;
        characterController.Move(Vector3.zero);
        //boss.GetComponent<Boss_movements>().bossMoveFlag = false;
        boss.GetComponent<Boss_movements>().setMoveBool(false);
       // playAudio(2);
        while (currentTime < seconds)
        {
            Debug.Log("CO ROUTINE LOOP");
            currentTime += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    IEnumerator killPlayerFall(float seconds)
    {
        float currentTime = 0;
        playerMoveFlag = false;
        playAudio(2);
        boss.GetComponent<Boss_movements>().setMoveBool(false);
       

        while (currentTime < seconds)
        {
            Debug.Log("CO ROUTINE LOOP Fall");
            currentTime += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

     public void Fire()
    {
        
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);
        
        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * 50;
        // Destroy the bullet after 2 seconds
        playAudio(0);
        Destroy(bullet, 5f);
        
    }
}
