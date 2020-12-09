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
    public bool cenouraPraCima = true;

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
        //Debug.Log("jogando cenoura");
        if (indexPosicao == 0 || indexPosicao == 2) {
            jogarCenouraAnimacao();
        } else {
            // Mudar para função de jogar gelo
            jogarCenouraAnimacao();
        }
    }

    void jogarCenouraAnimacao() {
        BossNeve.instance.animator.SetTrigger("jogandoCenoura");
        StartCoroutine(jogarCenoura());
    }

    IEnumerator jogarCenoura() {
        yield return new WaitForSeconds(1.01f);

        bool direitaBoss = BossNeve.instance.isBossOlhandoDireita();

        Transform cenouraRandom = posicoesCenoura[direitaBoss ? 0 : 1];

        Quaternion rotation = direitaBoss ? Quaternion.AngleAxis(0, new Vector3(0, 0, 0)) : Quaternion.AngleAxis(180, new Vector3(0, 180, 0));

        int i = 3;

        while (i-- > 0) {
            Vector3 positionRandom = new Vector3(cenouraRandom.position.x, 0, cenouraRandom.position.z);
            if (cenouraPraCima) {
                positionRandom.y = (i + 1.5f);
            } else {
                positionRandom.y = (i + 0.5f) * -1;
            }
            Debug.Log(positionRandom);
            Rigidbody2D temp = Instantiate(cenouraPrefab, positionRandom, rotation).GetComponent<Rigidbody2D>();
            temp.velocity = new Vector2(direitaBoss ? velocidadeCenoura : velocidadeCenoura * -1, 0);
        }

        cenouraPraCima = !cenouraPraCima;

        Invoke("mudarPosicaoBossRandom", 1f);
    }

    void flipCenoura() {
        Vector3 scale = cenouraPrefab.transform.localScale;
        cenouraPrefab.transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
    }

    void jogarBola() {
        Vector3 posicaoRandom = posicoesCenoura[Random.Range(0, posicoesBola.Length)].localPosition;
        Rigidbody2D temp = Instantiate(bolaPrefab, posicaoRandom, transform.localRotation).GetComponent<Rigidbody2D>();
        temp.velocity = new Vector2(0, velocidadeBola);
    }

}
