using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Animator animator;
    private int levelToLoad;

    // To check if the Play Button is clicked
    GameObject lastClicked;
    Ray ray;
    RaycastHit rayHit;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update () {
        if(Input.GetMouseButtonDown(0)) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out rayHit, 100)) {
                //Debug.Log(rayHit.transform.gameObject.name);

                lastClicked = rayHit.collider.gameObject;
                if (lastClicked != null && lastClicked.name.Equals("carpet")) {
                    FadeToLevel(1);
                }
            }
        }
	}

    public void FadeToLevel(int levelIndex) {
        levelToLoad = levelIndex;              
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete() {
        SceneManager.LoadScene(levelToLoad);
        animator.SetTrigger("FadeIn");
    }
    public void OnFadeInComplete() {

    }
}
