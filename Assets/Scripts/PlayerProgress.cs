using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgress : MonoBehaviour {

    // References to the Transform of the UI Components in the RadialProgressBar gameObject.
    public Slider loadingBar;

    // Values holding current progress and speed of progress.
    public float currentProgress;
    [SerializeField] private float speed;

    // Free-Fall check to stop updating progress
    public bool isInFreeFall = false;

    public bool calledSetDeath = false;

    // Player Animator Reference
    Animator animator;

    // Player Motor Reference
    PlayerMotor playerMotor;

    // Use this for initialization
    void Start () {
        loadingBar.value = 0;

        animator = GetComponent<Animator>();
        playerMotor = GetComponent<PlayerMotor>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if(currentProgress < 100 && !isInFreeFall && !calledSetDeath) {
            currentProgress += speed * Time.deltaTime;
        }
        else if (isInFreeFall) {
            animator.SetTrigger("Free Fall");
            SetDeath();
            playerMotor.RestartCurrentScene();
        }

        if(!isInFreeFall) {
            loadingBar.value = currentProgress / 100;
        }
	}

    // Implement player death
    public void SetDeath() {
        calledSetDeath = true;
    }
}
