using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {
    public GameObject myCamera;
    public bool bossCamera;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (bossCamera) {
                GameObject bgFase = GameObject.Find("BackgroundFase");
                if (bgFase != null) {
                    bgFase.SetActive(false);
                }
                SalaBoss.instance.iniciarBoss();
            }
            CameraController.instance.EnableCamera(myCamera, bossCamera);
        }
    }
}
