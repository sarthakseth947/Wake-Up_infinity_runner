using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
    public GameObject GameoverScreen;
    public GameObject Continuescreen;
    bool help = true;
    bool help1 = true;
    protected Joystick Joystick;
    public joybutton Joybutton;
    public joybutton Change;
    public joybutton Shoot;
    public Text Gameover;
    public Text missiles;
    public Text bullets;
    Animator anim;
    public Animator anim2;
    private int a = 10;
    private int b = 50;
    public static int j = 0;
    public GameObject projectile;
    public GameObject projectile2;
    public Transform shotpoint;
    public bool InAir = false;
    public float speed;
    private Rigidbody rb;
    public int jumpforce;
    public UnityEngine.UI.Slider healthbar1;
    private bool left;
    private Quaternion playerRotation;
    private Quaternion projectileRotation;
    public bool playerMove = true;
    public GameObject enemy;
    DemonCOntroller enemyScript;
    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        missiles.text = "Missiles Left :" + a;
        bullets.text = "Bullets Left :" + b;
        rb = GetComponent<Rigidbody>();
        enemyScript = enemy.GetComponent<DemonCOntroller>();

    }

    void Start()
    {
        help = true;
 
        Joystick = FindObjectOfType<Joystick>();


        left = true;
        playerRotation = Quaternion.identity;
        projectileRotation = Quaternion.identity;
        playerRotation.y = 180;
        projectileRotation.y = 0;

        healthbar1.value = 50;

    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "fire" || other.gameObject.tag == "Enemy")
        {
            healthbar1.value = healthbar1.value - 10;


        }
        if (other.gameObject.tag == "Untagged" && InAir == true)
        {
            InAir = false;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Untagged" && InAir == true)
        {
            InAir = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        InAir = true;
    }

    private void Update()
    {




   

    }
    // Update is called once per frame
    void FixedUpdate()
    {


        if (Input.GetKeyUp(KeyCode.R) )
        {
            SceneManager.LoadScene("Level 1 Boss");
        }

        if (healthbar1.value <= 0)
        {
           
         
            playerMove = false;
            enemyScript.enemyMove = false;
            while (help)
            {
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                anim.SetTrigger("Die");
                anim2.SetTrigger("victory");
                GameoverScreen.gameObject.SetActive(true);
                help = false;
            }

        }
        if (enemyScript.healthbar.value <= 0)
        {
      
            playerMove = false;
            while (help1)
            {

                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                anim.SetTrigger("victory");
                anim2.SetTrigger("Die");
                Continuescreen.gameObject.SetActive(true);
                help1 = false;
            }
        }
        if (playerMove)
        {

            if ((Input.GetKeyUp("space") || Shoot.pressed) && Time.time >= nextFire)
            {
                if (j == 0)
                {
                    if (a > 0)
                    {
                        GameObject newProjectile = Instantiate(projectile, shotpoint.position, projectileRotation) as GameObject;
                        newProjectile.transform.localScale = (new Vector3(0.3f, 0.3f, 0.3f));
                        nextFire = Time.time + fireRate;
                        a--;
                        missiles.text = "Missiles Left :" + a;
                    }
                }
                else
                {
                    if (b > 0)
                    {
                        GameObject newProjectile = Instantiate(projectile2, shotpoint.position, projectileRotation) as GameObject;
                        newProjectile.transform.localScale = (new Vector3(2.0f, 2.0f, 2.0f));
                        nextFire = Time.time + fireRate;
                        b--;
                        bullets.text = "Bullets Left :" + b;
                    }
                }
            }

    




            if (Input.GetKeyUp("down") || Change.pressed)
            {
                if (j == 0)
                {
                    transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                    j++;

                }
                else
                {
                    transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                    j--;

                }
            }

            /*if (transform.position.z>112){
            GetComponent<Collider>().isTrigger = true;
            Gameover.gameObject.SetActive(true);
        }

        if (transform.position.z < -86)
        {
            GetComponent<Collider>().isTrigger = true;
            Gameover.gameObject.SetActive(true);
        }*/

            float moveHorizontal = Input.GetAxis("Horizontal");


            //if (!Mathf.Approximately (moveHorizontal, 0)) {
            if (moveHorizontal < 0)
            {
                playerRotation.y = 180;
                projectileRotation.y = 0;
                left = true;
            }
            else if (moveHorizontal > 0)
            {
                playerRotation.y = 0;
                projectileRotation.y = 180;
                left = false;
            }
            //}
            if (Joystick.Horizontal < 0)
            {
                playerRotation.y = 180;
                projectileRotation.y = 0;
                left = true;
            }
            else if (Joystick.Horizontal > 0)
            {
                playerRotation.y = 0;
                projectileRotation.y = 180;
                left = false;
            }

            transform.rotation = playerRotation;

            if (left)
            {
                transform.Translate(0, 0, -moveHorizontal * 12f * Time.deltaTime);
                transform.Translate(0, 0, (-Joystick.Horizontal * 12f * Time.deltaTime));
            }

            else
            {
                transform.Translate(0, 0, moveHorizontal * 12f * Time.deltaTime);
                transform.Translate(0, 0, (Joystick.Horizontal * 12f * Time.deltaTime));
            }

            if (Input.GetKeyUp("up") && InAir == false)
            {
                anim.SetTrigger("jump");
                print("its working");
                Vector3 a = new Vector3(0, 8f, 0);
                rb.AddForce(0, jumpforce, 0, ForceMode.Impulse);

            }
            if (Joybutton.pressed && InAir == false)
            {
                anim.SetTrigger("jump");
                print("its working");
                Vector3 a = new Vector3(0, 8f, 0);
                rb.AddForce(0, jumpforce, 0, ForceMode.Impulse);

            }

        }


    }
  

}







   