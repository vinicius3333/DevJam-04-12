﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;

    public GameObject[] cameras;
    public float freezeTime = 1f;

    private void Awake() {
        instance = this;
    }

    public void EnableCamera(GameObject camera, bool bossCamera) {
        if (camera.activeInHierarchy) return;

        for (int i = 0; i < cameras.Length; i++) {
            cameras[i].SetActive(false);
        }

        camera.SetActive(true);

        StartCoroutine(FreezingTime());

        if (bossCamera) {
            StartCoroutine(moverSozinho());
        }
    }

    IEnumerator FreezingTime() {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(freezeTime);

        Time.timeScale = 1;
    }

    IEnumerator moverSozinho() {
        PlayerController.instance.moverSozinho = true;

        yield return new WaitForSecondsRealtime(2.3f);

        PlayerController.instance.moverSozinho = false;
    }
}
