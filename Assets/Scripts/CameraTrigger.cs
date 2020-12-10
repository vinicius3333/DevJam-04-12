using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {
    public GameObject myCamera;
    public bool bossCamera;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (bossCamera) {
                SalaBoss.instance.iniciarBoss();
            }
            CameraController.instance.EnableCamera(myCamera, bossCamera);
        }
    }
}
