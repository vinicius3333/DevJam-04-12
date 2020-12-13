using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacoPapaiNoelController : MonoBehaviour {
    public GameObject presentePrefab;

    public GameObject botaoEspacoPrefab;

    private Animator animator;

    public int numeroAnimacao = 10;

    public float aguardarAnimacao = 0.2f;

    public float presenteForce = 0.4f;

    public float zoomPresente = 0.5f;

    private bool podeApertarEspaco = false;

    public bool isEnd = false;

    private GameController gameController;


    void Start() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (podeApertarEspaco) {
            if (Input.GetButtonDown("Jump")) {
                CameraController.instance.zoomPlayer();
                PlayerController.instance.isPlayerParado = false;
                podeApertarEspaco = false;
                Destroy(presentePrefab.gameObject);
                Destroy(botaoEspacoPrefab.gameObject);
                Destroy(gameObject);
                if (isEnd) {
                    gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
                    PlayerController.instance.moverSozinho = false;
                    gameController.proximaCena();
                    return;
                }
                StartCoroutine(moverSozinho(5f));
            }
        }
    }

    IEnumerator moverSozinho(float timeToWalk) {
        PlayerController.instance.moverSozinho = true;

        yield return new WaitForSecondsRealtime(timeToWalk);

        PlayerController.instance.moverSozinho = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            PlayerController.instance.isPlayerParado = true;
            animator.SetTrigger("Coletar");
            StartCoroutine(criarPresente());
        }
    }

    private IEnumerator criarPresente() {
        presentePrefab = GameObject.Instantiate(presentePrefab, transform.localPosition, transform.localRotation);
        Transform transformTemp = presentePrefab.GetComponent<Transform>();


        for (int i = 0; i < numeroAnimacao; i++) {
            transformTemp.position += Vector3.up * presenteForce;
            yield return new WaitForSecondsRealtime(aguardarAnimacao);
        }

        CameraController.instance.zoomItem(transformTemp, zoomPresente);

        botaoEspacoPrefab = GameObject.Instantiate(botaoEspacoPrefab, transform.localPosition + Vector3.right + Vector3.up, transform.localRotation);

        yield return new WaitForSecondsRealtime(1.5f);

        podeApertarEspaco = true;
    }
}
