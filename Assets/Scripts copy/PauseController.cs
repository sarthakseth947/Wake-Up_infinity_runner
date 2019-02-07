using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseController : MonoBehaviour {

    // Reference to pause-play image
    public Image pausePlayImage;

    // Reference to the pause sprite
    public Sprite pauseSprite;

    // Reference to the play sprite
    public Sprite playSprite;

    public bool isPaused;

	// Use this for initialization
	void Start () {
        isPaused = false;
        pausePlayImage.sprite = pauseSprite;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            isPaused = !isPaused;
        }

        if (isPaused) {
            pausePlayImage.sprite = playSprite;
            Time.timeScale = 0;
        }
        else {
            pausePlayImage.sprite = pauseSprite;
            Time.timeScale = 1;
        }

    }

    // Pause on button click
    public void Pause() {
        Debug.Log("Button pressed!");

        isPaused = !isPaused;

        if (isPaused) {
            pausePlayImage.sprite = playSprite;
            Time.timeScale = 0;
        }
        else {
            pausePlayImage.sprite = pauseSprite;
            Time.timeScale = 1;
        }
    }
}
