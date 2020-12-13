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

    private void Start() {
        _FadeController = FindObjectOfType(typeof(FadeController)) as FadeController;
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && gameOverMenu.activeSelf == false) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void TryAgain()
    {
        _AudioController.PlayFX(_AudioController.uiClick, 1f);
        _FadeController.FadeIn();
        if(_FadeController.isFadeComplete == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
        SceneManager.LoadScene(0);
    }
}
