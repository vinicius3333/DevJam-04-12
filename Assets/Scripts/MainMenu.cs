using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private AudioController _AudioController;
    public GameObject transition;
    public GameObject controlePanel;
    private FadeController _FadeController;
    private bool isWait;

    private void Start() {
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        transition.SetActive(true);
        _FadeController = FindObjectOfType(typeof(FadeController)) as FadeController;
    }

    public void PlayGame() {
        if (isWait == true) { return; }
        StartCoroutine("WaitToPlayGame");
        _AudioController.PlayFX(_AudioController.uiClick, 1f);
    }

    public void QuitGame() {
        _AudioController.PlayFX(_AudioController.uiClick, 1f);
        Application.Quit();
    }

    public void OpenPanel() {
        controlePanel.SetActive(!controlePanel.activeSelf);
        _AudioController.PlayFX(_AudioController.uiClick, 1f);
    }

    IEnumerator WaitToPlayGame() {
        isWait = true;
        _AudioController.ChangeMusic(_AudioController.level1);
        _FadeController.FadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
