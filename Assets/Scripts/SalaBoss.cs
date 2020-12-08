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
    private bool isLookLeftCenoura = false;
    public float velocidadeBola = 5f;

    void Start() {
        instance = this;
    }

    void mudarPosicaoBossRandom() {
        indexPosicao = Random.Range(0, posicoesBoss.Length);
        while (ultimaPosicao == indexPosicao && posicoesBoss.Length > 0) {
            indexPosicao = Random.Range(0, posicoesBoss.Length);
        }
        ultimaPosicao = indexPosicao;
        BossNeve.instance.pularBoss(posicoesBoss[indexPosicao]);
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

        Transform cenouraRandom = posicoesCenoura[Random.Range(0, posicoesCenoura.Length)];

        if (cenouraRandom.localPosition.x > 0 && cenouraPrefab.transform.localScale.x > 0) {
            flipCenoura();
        } else if (cenouraRandom.localPosition.x < 0 && cenouraPrefab.transform.localScale.x < 0) {
            flipCenoura();
        }

        //cenouraRandom.localScale = new Vector3(5, 5, 1);

        Rigidbody2D temp = Instantiate(cenouraPrefab, cenouraRandom.position, transform.localRotation).GetComponent<Rigidbody2D>();

        isLookLeftCenoura = cenouraRandom.localPosition.x > 0;

        temp.velocity = new Vector2(cenouraRandom.localPosition.x > 0 ? velocidadeCenoura * -1 : velocidadeCenoura, 0);

        Invoke("mudarPosicaoBossRandom", 1f);
        //Invoke("jogarCenoura", Random.Range(1f, 1.5f));
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
