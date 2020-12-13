using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {
    public GameObject myCamera;
    public bool bossCamera;

    public bool bossRena;

    public bool conclusao;

    public GameObject AllRenaPrefab;

    private GameObject bgFase;

    private void Start() {
        bgFase = GameObject.Find("BackgroundFase");
    }

    // Desculpa por esse código horrível, mas fazer o que, né
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (bossCamera) {
                if (bgFase != null) {
                    bgFase.SetActive(false);
                }
                SalaBoss.instance.iniciarBoss();
            }
            if (conclusao) {
                if (bgFase != null) {
                    bgFase.SetActive(true);
                }
            }
            if (bossRena) {
                StartCoroutine(ativarRena());
            }
            CameraController.instance.EnableCamera(myCamera, bossCamera, bossRena, conclusao);
        }
    }

    IEnumerator ativarRena() {
        yield return new WaitForSecondsRealtime(0.9f);
        AllRenaPrefab.SetActive(true);
    }
}
