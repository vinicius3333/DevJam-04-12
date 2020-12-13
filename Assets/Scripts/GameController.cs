using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject transition;
    private FadeController _FadeController;
    private bool isWait;

    private GameMaster gameMaster;


    private void Start() {
        transition.SetActive(true);
        _FadeController = FindObjectOfType(typeof(FadeController)) as FadeController;
        gameMaster = GameObject.FindWithTag("GM").GetComponent<GameMaster>();
    }

    public void resetarCena() {
        if (isWait == true) { return; }
        StartCoroutine("WaitToReset");
    }

    IEnumerator WaitToReset() {
        isWait = true;
        _FadeController.FadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void proximaCena() {
        gameMaster.posicaoPlayer = Vector2.zero;
        StartCoroutine(WaitToNext());
    }

    IEnumerator WaitToNext() {
        isWait = true;
        _FadeController.FadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
