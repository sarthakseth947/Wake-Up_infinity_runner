using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    // The amount of health the player starts the game with.
    public int startingHealth = 100;

    // The current health the player has.
    public int currentHealth;

    // Reference to the UI's health bar.
    public Slider healthSlider;

    // Reference to the Animator component.
    Animator anim;

    // Reference to the player's movement.
    PlayerMotor playerMotor;

    // Reference to the player's progress
    PlayerProgress playerProgress;

    // True when the player gets damaged.
    bool damaged;

    // Whether the player is dead.
    public bool isDead = false;


    // Use this for initialization
    void Start () {
        // Setting up the references.
        anim = GetComponent<Animator>();
        playerMotor = GetComponent<PlayerMotor>();
        playerProgress = GetComponent<PlayerProgress>();

        // Set the initial health of the player.
        currentHealth = startingHealth;
    }
	
	// Update is called once per frame
	void Update () {
        damaged = false;
	}

    // When player takes a damage
    public void TakeDamage(int amount) {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // If the player has lost all its health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead) {
            // ...player should die.
            //Death();
            //playerProgress.isInFreeFall = true; // not exactly, but do the same functions as if the player's in free fall

            StartCoroutine(KillPlayer(4));
        }
    }
    // When player dies
   /*public void Death() {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Tell the animator that the player is dead.
        anim.SetTrigger("Die");

        // Turn off the movement script.
        playerMotor.enabled = false;
    }*/

    IEnumerator KillPlayer(float seconds) {
        float currentTime = 0;
        playerMotor.playerMoveFlag = false;

        // Set the death flag so this function won't be called again.
        isDead = true;

        // Tell the animator that the player is dead.
        anim.SetTrigger("Die");

        playerProgress.SetDeath();

        while(currentTime < seconds) {
            currentTime += Time.deltaTime;
            yield return null;
        }


        playerMotor.RestartCurrentScene();

    }
}
