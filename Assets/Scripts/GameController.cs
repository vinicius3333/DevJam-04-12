using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private AudioController _AudioController;
    public GameObject transition;
    private FadeController _FadeController;
    private bool isWait;
    public bool isLevel2;

    private GameMaster gameMaster;


    private void Start() {
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
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
        _AudioController.ChangeMusic(_AudioController.level1);
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
        if(isLevel2 == false)
        {
            _AudioController.ChangeMusic(_AudioController.level2);
        }
        else
        {
            _AudioController.ChangeMusic(_AudioController.lastCene);
        } 
        _FadeController.FadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
