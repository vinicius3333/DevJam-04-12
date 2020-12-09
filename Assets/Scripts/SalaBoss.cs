using System.Collections;
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

    void Start() {
        instance = this;
    }

    void mudarPosicaoBossRandom() {
        indexPosicao = Random.Range(0, posicoesBoss.Length);
        while (ultimaPosicao == indexPosicao && posicoesBoss.Length > 0) {
            indexPosicao = Random.Range(0, posicoesBoss.Length);
        }
        ultimaPosicao = indexPosicao;
        StartCoroutine(BossNeve.instance.pularBoss(posicoesBoss[indexPosicao]));
    }

    public void encostarPlataforma() {
        BossNeve.instance.animator.SetBool("onAir", false);
        jogarObjetosTela();
    }

    public void jogarObjetosTela() {
        if (indexPosicao == 0 || indexPosicao == 2) {
            //            jogarCenouraAnimacao();
            jogarBolaAnimacao();
        } else {
            jogarBolaAnimacao();
        }
    }

    void jogarCenouraAnimacao() {
        BossNeve.instance.animator.SetTrigger("jogandoCenoura");
        //StartCoroutine(jogarCenoura());
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
    }

    void flipCenoura() {
        Vector3 scale = cenouraPrefab.transform.localScale;
        cenouraPrefab.transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
    }

    void jogarBolaAnimacao() {
        BossNeve.instance.animator.SetTrigger("jogandoBola");
    }

    public void jogarBola() {
        Transform cenouraRandom = posicoesBola[Random.Range(0, posicoesBola.Length)];

        Vector3 positionRandom = cenouraRandom.position;
        //positionRandom.y = (i + cenouraPosicoes[indexCenoura]);
        Rigidbody2D temp = Instantiate(bolaPrefab, positionRandom, bolaPrefab.transform.rotation).GetComponent<Rigidbody2D>();
        //temp.velocity = new Vector2(direitaBoss ? velocidadeCenoura : velocidadeCenoura * -1, 0);
    }

}
