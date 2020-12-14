using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    private AudioController _AudioController;
    private FadeController _FadeController;
    public static bool GameIsPaused;
    public GameObject pauseMenuUI;
    public GameObject gameOverMenu;
    private bool isFadeComplete;

    private void Start() {
        _FadeController = FindObjectOfType(typeof(FadeController)) as FadeController;
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
    }

    void Update() {
        isFadeComplete = _FadeController.isFadeComplete;
        if (Input.GetKeyDown(KeyCode.Escape) && gameOverMenu.activeSelf == false && isFadeComplete == true) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void TryAgain() {
        _AudioController.PlayFX(_AudioController.uiClick, 1f);
        _FadeController.FadeIn();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume() {
        _AudioController.PlayFX(_AudioController.uiClick, 1f);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void GoMenu() {
        _AudioController.PlayFX(_AudioController.uiClick, 1f);
        Time.timeScale = 1f;
        GameIsPaused = false;
        _AudioController.ChangeMusic(_AudioController.title);
        _FadeController.FadeIn();
        SceneManager.LoadScene(1);
    }
}
