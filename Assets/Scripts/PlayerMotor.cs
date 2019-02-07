using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Boundary properties
[System.Serializable]
public class Boundary {
    public float xMin, xMax;
}

// Collision properties
[System.Serializable]
public class Collisions {
    public bool didCollide = false;         // Stumble and Jump Stumble
    public bool didFallFlat = false;        // Trip
    public bool didCollideSideways = false; // Trip Sideways
}

// HealthBar Damage properties
[System.Serializable]
public class DamageValues {
    public int jumpStumbleDamage = 20;
    public int tableStumbleDamage = 10;
    public int tripOverDamage = 5;
    public int tripSideWaysDamage = 5;
}

// Enable/Disable Buttons Properties
[System.Serializable]
public class EnableDisableButtons {
    public bool isUpArrowEnabled = true;
    public bool isDownArrowEnabled = true;
}

public class PlayerMotor : MonoBehaviour {

    private Animator animator;
    private CharacterController controller;
    protected Joystick joystick;
	public bool reachedEnd = false;
    Vector3 moveDirection = Vector3.zero;

    // Boundary reference
    public Boundary boundary;

    // Collisions reference
    public Collisions collisions;

    // EnableDisable reference
    public EnableDisableButtons enableDisable;

    // DamageValues reference
    public DamageValues damageValues;

    public float speed;
    public float speedXAxis;
    public float jumpSpeed;

    public float gravity;

    // Reference to the player's health.
    PlayerHealth playerHealth;

    // Reference to the player's progress
    PlayerProgress playerProgress;

    // Reference to the player's audio source
    public AudioSource audioSource;

    // Reference to the game audio's source
    GameAudioController gameAudio;

    // Flag for progress
    public bool playerMoveFlag = true;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        joystick = FindObjectOfType<Joystick>();

        playerHealth = GetComponent<PlayerHealth>();
        playerProgress = GetComponent<PlayerProgress>();

        gameAudio = GameObject.FindGameObjectWithTag("Game Audio").GetComponent<GameAudioController>();
    }

    // Update is called once per frame
    void Update() {
		if (!reachedEnd) {			
			if (!playerHealth.isDead && playerMoveFlag) {
				if (controller.isGrounded) { 

					if (collisions.didCollide) {
						moveDirection = new Vector3 (
							(Input.GetAxis ("Horizontal") + joystick.Horizontal) * speedXAxis,
							0.0f,
							0.0f
						);
					}
					if (collisions.didFallFlat || collisions.didCollideSideways) {
						moveDirection = Vector3.zero;
					}

					if (!collisions.didCollide &&
					    !collisions.didFallFlat &&
					    !collisions.didCollideSideways) {
						moveDirection = new Vector3 (
							(Input.GetAxis ("Horizontal") + joystick.Horizontal) * speedXAxis,
							0,
							speed
						);
					}

					moveDirection = transform.TransformDirection (moveDirection);

					if ((Input.GetKey (KeyCode.UpArrow) || joystick.Vertical > 0.5f) && enableDisable.isUpArrowEnabled) {
						animator.SetTrigger ("Jump");
						moveDirection.y = jumpSpeed;
						moveDirection.z += speed * 15;

					}
					/*if ((Input.GetKey(KeyCode.DownArrow) || joystick.Vertical < -0.5f) && enableDisable.isDownArrowEnabled) {
	                    animator.SetTrigger("Slide");
	                    moveDirection.z += speed;
	                }*/
				} else {
					moveDirection.x = (Input.GetAxis ("Horizontal") + joystick.Horizontal) * speed;
					moveDirection.z = speed;
				}
				moveDirection.y -= gravity * Time.deltaTime;

				if (((int)Mathf.Round (transform.position.z) % 250) == 0) {
					//Debug.Log("Increase speed");
					speed += 1.0f;
					moveDirection.z = speed;
				}

				controller.Move (moveDirection * Time.deltaTime);
			}
			ClampPosition ();

			// Trigger falling animation when player goes below ground level
			// So player has died and we reload the scene after playing the death audio
			/*if (transform.position.y < -20.0f) {

	            //animator.SetTrigger("Free Fall");

	            playerHealth.healthSlider.value = 0;
	            playerProgress.isInFreeFall = true;

	            PlayDeathAudio();

	            // Reload current scene
	            StartCoroutine("ReloadScene");
	        }*/

			// Transition to the next scene if the player is done with the level
			/*if(playerProgress.currentProgress >= 100) {
	            LoadNextScene();
	        }*/
		}
    }

    private void ClampPosition() {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax),
            transform.position.y,
            transform.position.z
        );
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("TableStumble")) {

            // Trigger animation
            animator.SetTrigger("Stumble");

            // Decrease health
            DamagePlayer("Stumble");

            // Play impact sound
            PlayAudio(Resources.Load("Sounds/JustImpacts-Extension2_Metal_Hit_Crash_200") as AudioClip);
        }

        if (other.CompareTag("TripOver")) {

            // Trigger animation
            animator.SetTrigger("Trip");

            // Decrease health
            DamagePlayer("Trip");

            // Play impact sound
            PlayAudio(Resources.Load("Sounds/Just_Impacts_Extension-I_163") as AudioClip);

        }

        if (other.CompareTag("TripSideways")) {

            // Trigger animation
            animator.SetTrigger("Stumble Sideways");

            // Decrease health
            DamagePlayer("Stumble Sideways");

            // Play impact sound
            PlayAudio(Resources.Load("Sounds/JustImpacts-Extension2_Misc_Hits_044") as AudioClip);
        }

        if(other.CompareTag("GateStumble")) {

            // Trigger animation
            animator.SetTrigger("Stumble");

            // Decrease health
            DamagePlayer("Jump Stumble");

            // Play impact sound
            PlayAudio(Resources.Load("Sounds/Just_Impacts_Extension-I_171") as AudioClip);
        }

        // To trigger falling animation
        if(other.CompareTag("falling")) {

            playerHealth.healthSlider.value = 0;
            playerProgress.isInFreeFall = true;

            PlayDeathAudio();

            // Reload current scene
            StartCoroutine("ReloadScene");
        }

		if (other.CompareTag ("FinalStop")) {
			animator.SetTrigger ("StopFinal");
			Animator fade = GameObject.FindGameObjectWithTag("HUD").GetComponent<Animator>();
			fade.SetTrigger ("FadeOut");
			LoadNextScene ();
			reachedEnd = true;
		}
    }
    private void DamagePlayer(string animationTrigger) {

        // If the player has health to lose...
        if (playerHealth.currentHealth > 0) {
            // ... damage the player based on animation triggered.

            if(animationTrigger == "Stumble") {
                playerHealth.TakeDamage(damageValues.tableStumbleDamage);
            }
            else if(animationTrigger == "Trip") {
                playerHealth.TakeDamage(damageValues.tripOverDamage);
            } 
            else if(animationTrigger == "Stumble Sideways") {
                playerHealth.TakeDamage(damageValues.tripSideWaysDamage);
            }
            else if (animationTrigger == "Jump Stumble") {
                playerHealth.TakeDamage(damageValues.jumpStumbleDamage);
            }
        }
        if(playerHealth.isDead) {
            PlayDeathAudio();
            RestartCurrentScene();
        }
    }
    public void PlayAudio(AudioClip clip) {

        audioSource.clip = clip;
        audioSource.Play();
    }

    // Stop current audio and play death audio
    private void PlayDeathAudio() {
        gameAudio.audioSource.Stop();

        gameAudio.audioSource.clip = Resources.Load("Sounds/Just_Transitions_Creepy-008") as AudioClip;
        gameAudio.audioSource.Play();
    }
    // Transition to boss scene given the level index when player's progress is done
    private void TransitionToBossScene(int levelIndex) {
        SceneManager.LoadScene(levelIndex);
    }

    // Reload current scene after player dies AFTER 10 SECONDS!
    public void RestartCurrentScene() {
        StartCoroutine("ReloadScene");
    }
    IEnumerator ReloadScene() {
        yield return new WaitForSeconds(3);

        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    // Transition to the next scene after progress is done
    public void LoadNextScene() {
        enabled = false; // stop playerMotor script from running
        //animator.enabled = false; // stop running animation or any animation for that matter
        StartCoroutine("LoadScene");
       //LoadScene();
    }
    IEnumerator LoadScene() {
		yield return new WaitForSeconds(5);
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene + 1, LoadSceneMode.Single);
    }
    /*private void LoadScene() {
        animator.runtimeAnimatorController = Resources.Load("Animator/LevelChanger") as RuntimeAnimatorController;
        animator.SetTrigger("FadeOut");
    }
    public void OnFadeOutComplete() {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene + 1, LoadSceneMode.Single);
    }*/
}
