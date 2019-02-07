using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DescriptionLevels : MonoBehaviour {

    public Button nextButton;

	// Use this for initialization
	void Start () {
        nextButton.onClick.AddListener(GoToNextScene);
	}
	
	// Update is called once per frame
	void GoToNextScene () {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene + 1, LoadSceneMode.Single);
    }
}
