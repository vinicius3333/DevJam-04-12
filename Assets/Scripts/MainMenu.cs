using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject transition;
    private FadeController _FadeController;
    private bool isWait;

    private void Start() {
         transition.SetActive(true);
        _FadeController = FindObjectOfType(typeof(FadeController)) as FadeController;
    }

    public void PlayGame() {
        if(isWait == true) {return;}
        StartCoroutine("WaitToPlayGame");
    }

    public void QuitGame() {
        Application.Quit();
    }

    IEnumerator WaitToPlayGame()
    {
        isWait = true;
        _FadeController.FadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
