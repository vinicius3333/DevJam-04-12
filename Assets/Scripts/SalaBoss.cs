﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalaBoss : MonoBehaviour {
    public static SalaBoss instance;

    public Transform[] posicoesBoss;

    public GameObject cenouraPrefab;
    public GameObject bolaPrefab;

    public Transform[] posicoesCenoura;
    public Transform[] posicoesBola;

    private int indexPosicao = 0;
    private int ultimaPosicao = 0;

    public float velocidadeCenoura = 5f;
    public float velocidadeBola = 5f;
    private float[] cenouraPosicoes = new float[] { 1.5f, 1.0f, 0.5f };
    private int indexCenoura = 0;
    private bool bolaJogada = false;

    void Start() {
        instance = this;
    }

    void mudarPosicaoBossRandom() {
        indexPosicao = Random.Range(0, posicoesBoss.Length);
        while (ultimaPosicao == indexPosicao && posicoesBoss.Length > 0) {
            indexPosicao = Random.Range(0, posicoesBoss.Length);
        }
        ultimaPosicao = indexPosicao;
        BossNeve.instance.proximaPosicao = posicoesBoss[indexPosicao];
        BossNeve.instance.animator.SetTrigger("jump");
    }

    public void encostarPlataforma() {
        BossNeve.instance.animator.SetBool("onAir", false);
        jogarObjetosTela();
    }

    public void jogarObjetosTela() {
        if (indexPosicao == 0 || indexPosicao == 2) {
            jogarCenouraAnimacao();
        } else {
            jogarBolaAnimacao();
        }
    }

    void jogarCenouraAnimacao() {
        BossNeve.instance.animator.SetTrigger("jogandoCenoura");
    }

    public void jogarCenoura() {

        bool direitaBoss = BossNeve.instance.isBossOlhandoDireita();

        Transform cenouraRandom = posicoesCenoura[direitaBoss ? 0 : 1];

        Quaternion rotation = direitaBoss ? Quaternion.AngleAxis(0, new Vector3(0, 0, 0)) : Quaternion.AngleAxis(180, new Vector3(0, 180, 0));

        int i = 3;

        while (i-- > 0) {
            Vector3 positionRandom = new Vector3(cenouraRandom.position.x, 0, cenouraRandom.position.z);
            positionRandom.y = (i + cenouraPosicoes[indexCenoura]);
            Rigidbody2D temp = Instantiate(cenouraPrefab, positionRandom, rotation).GetComponent<Rigidbody2D>();
            temp.velocity = new Vector2(direitaBoss ? velocidadeCenoura : velocidadeCenoura * -1, 0);
        }

        if (indexCenoura == 2) indexCenoura = 0;
        else indexCenoura++;
        StartCoroutine(bossAguarde());
    }

    void flipCenoura() {
        Vector3 scale = cenouraPrefab.transform.localScale;
        cenouraPrefab.transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
    }

    void jogarBolaAnimacao() {
        BossNeve.instance.animator.SetTrigger("jogandoBola");
    }

    public void jogarBola() {
        if (bolaJogada) {
            var bolaNeve = GameObject.FindWithTag("BolaNeve");
            if (bolaNeve != null) {
                Destroy(bolaNeve);
            }
        }

        int indexAleatorio = Random.Range(0, posicoesBola.Length);
        Vector3 posicaoPlayer = PlayerController.instance.transform.position;
        Vector3 posicaoLonge = posicoesBola[indexAleatorio].position;

        if (Mathf.Ceil(posicaoPlayer.x) - Mathf.Ceil(posicaoLonge.x) < 5 || Mathf.Ceil(posicaoPlayer.x) - Mathf.Ceil(posicaoLonge.x) < -5) {
            if (indexAleatorio == posicoesBola.Length - 1) {
                indexAleatorio = 0;
            } else {
                indexAleatorio++;
            }
        }

        Rigidbody2D temp = Instantiate(bolaPrefab, posicaoLonge, bolaPrefab.transform.rotation).GetComponent<Rigidbody2D>();
        bolaJogada = true;
        StartCoroutine(bossAguarde());
    }

    public IEnumerator bossAguarde() {
        yield return new WaitForSeconds(Random.Range(2.5f, 3.5f));
        mudarPosicaoBossRandom();
    }

    public void destruirBolaNeve(GameObject gameObject) {
        Destroy(gameObject);
        bolaJogada = false;
    }

}
