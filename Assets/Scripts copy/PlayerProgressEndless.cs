using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgressEndless : MonoBehaviour {

    // References to the Transform of the UI Components in the RadialProgressBar gameObject.
	public Text score;
	public Text highscore;
	float highscoreVal;

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
		score = GameObject.FindGameObjectWithTag ("score").GetComponent<Text>() as Text;
		highscore = GameObject.FindGameObjectWithTag ("highscore").GetComponent<Text>() as Text;
		if (!PlayerPrefs.HasKey ("highscore")) {
			PlayerPrefs.SetFloat ("highscore", 0);
			Debug.Log ("Set");
		}
		highscoreVal = PlayerPrefs.GetFloat ("highscore");
		highscore.text = "Best Time: " + highscoreVal;
		Debug.Log (highscoreVal);
		score.text = "Time: 0";
        animator = GetComponent<Animator>();
        playerMotor = GetComponent<PlayerMotor>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if(!isInFreeFall && !calledSetDeath) {
            currentProgress += speed * Time.deltaTime;
        }
        else if (isInFreeFall) {
            animator.SetTrigger("Free Fall");
            SetDeath();
            playerMotor.RestartCurrentScene();
        }

        if(!isInFreeFall) {
			PlayerPrefs.SetFloat ("highscore", Mathf.Max(currentProgress,highscoreVal));
			score.text = "Time: " + currentProgress;
        }

	}

    // Implement player death
    public void SetDeath() {
        calledSetDeath = true;
    }
}
