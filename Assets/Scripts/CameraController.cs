using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;

    public GameObject[] cameras;
    public float freezeTime = 1f;

    public CinemachineVirtualCamera vcam;

    private bool isZooming = false;

    private float yVelocity = 0.0f;

    private float targetZoom;

    private float zoomFactor;

    private float zoomLerpSpeed = 10;

    private void Awake() {
        instance = this;
    }

    public void EnableCamera(GameObject camera, bool bossCamera, bool bossRena, bool conclusao) {
        if (camera.activeInHierarchy) return;

        for (int i = 0; i < cameras.Length; i++) {
            cameras[i].SetActive(false);
        }

        camera.SetActive(true);

        StartCoroutine(FreezingTime());

        if (bossCamera) {
            StartCoroutine(moverSozinho(2.3f));
        }

        if (conclusao) {
            StartCoroutine(moverSozinho(6f));
        }
    }

    private void Update() {
        if (vcam == null) getVcam();

        if (isZooming) {
            if (vcam.m_Lens.OrthographicSize < 1f) return;
            targetZoom -= 0.05f * zoomFactor;

            vcam.m_Lens.OrthographicSize = Mathf.SmoothDamp(vcam.m_Lens.OrthographicSize, targetZoom, ref yVelocity, Time.deltaTime * zoomLerpSpeed);
        }
    }

    private void getVcam() {
        for (int i = 0; i < cameras.Length; i++) {
            if (cameras[i].activeInHierarchy) {
                vcam = cameras[i].GetComponent<CinemachineVirtualCamera>();
            }
        }
    }

    IEnumerator FreezingTime() {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(freezeTime);

        Time.timeScale = 1;
    }

    IEnumerator moverSozinho(float timeToWalk) {
        PlayerController.instance.moverSozinho = true;

        yield return new WaitForSecondsRealtime(timeToWalk);

        PlayerController.instance.moverSozinho = false;
    }

    public void zoomItem(Transform transform, float zoom) {
        getVcam();
        vcam.Follow = transform;
        targetZoom = vcam.m_Lens.OrthographicSize;
        zoomFactor = zoom;
        isZooming = true;
    }

    public void zoomPlayer() {
        vcam.Follow = PlayerController.instance.transform;
        vcam.m_Lens.OrthographicSize = 5f;
        isZooming = false;
    }
}
