using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foca : MonoBehaviour {
    public enum EnemyState {
        PATRULHANDO,
        ATIRANDO
    }

    private PlayerController _PlayerController;
    private Rigidbody2D enemyRb;
    public EnemyState enemyCurrentState; //estado atual do inimigo
    public GameObject shotPrefab;
    public Transform shotPosition;

    public float timeInPoint; //tempo em cada ponto
    public float enemySpeed;
    public float enemyHP;

    public float shotSpeed;
    public float forceY;
    public float timeToShot;

    public float maxDistance;

    public Transform[] wayPoints; //pontons para percorrer, e tambem usado para definir a area que ele proteje
    public GameObject head; //cabeça do inimigo e responsavel pelo dano do pulo

    [Header("Controladores")]
    private bool isLockPlayer;
    private bool isShot;
    private bool isPlayerDetected; //se o player entrou na area de colisao

    private int idTarget = 0; //controla o objetivo
    private bool isCenter = false; //controla se está centralizado no ponto objetivo
    private Transform target; //objetivo
    private Vector3 direction; //direcao do movimento setado da posAtual para o objetivp

    private bool isLockLeft;

    private Animator animator;

    public float changeColorTimes = 6f;

    public float timeBetweenChangeColor = 0.15f;

    private SpriteRenderer spriteRenderer;

    public Color colorPadrao;
    public Color colorHit;
    private Rigidbody2D tempBall;

    public float forceBall;

    public Transform posicaoFinal;

    private Vector2 directionUpdate;

    public GameObject argolaPrefab;

    public bool isFocaArgola;

    private bool isDead = false;



    // Start is called before the first frame update
    void Start() {
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;

        enemyRb = GetComponentInChildren<Rigidbody2D>();
        target = wayPoints[idTarget];

        direction = Vector3.Normalize(transform.position - target.position);

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (isDead) return;
        //checa se o player está acima da cabeça
        if (_PlayerController.gameObject.transform.position.y > head.transform.position.y && head.activeSelf == false) {
            head.SetActive(true);
        } else if (_PlayerController.gameObject.transform.position.y < head.transform.position.y && head.activeSelf == true) {
            head.SetActive(false);
        }

        if (enemyCurrentState == EnemyState.ATIRANDO) {
            if (enemyHP == 0) return;
            if (isOlhandoDireita()) {
                transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 0));
            } else {
                transform.rotation = Quaternion.AngleAxis(180, new Vector3(0, 180, 0));
            }
        }
    }

    private void FixedUpdate() {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, _PlayerController.transform.position);

        if (distance > maxDistance) {
            enemyCurrentState = EnemyState.PATRULHANDO;
        } else if (distance <= maxDistance && isLockPlayer == false && enemyCurrentState == EnemyState.PATRULHANDO) {
            enemyCurrentState = EnemyState.ATIRANDO;
        }

        Vector2 posicaoAtual = transform.position;
        Vector2 PosicaoFinalTemp = posicaoFinal.position;
        directionUpdate = PosicaoFinalTemp - posicaoAtual;

        switch (enemyCurrentState) {
            case EnemyState.PATRULHANDO:
                Patrulha();
                StopCoroutine("ShotDelay");
                isLockPlayer = false;
                isShot = false;

                break;

            case EnemyState.ATIRANDO:
                isLockPlayer = true;
                enemyRb.velocity = Vector2.zero;
                if (isShot == false) {
                    Atirar();
                }
                break;
        }
    }

    private void OnBecameVisible() {
        shotPosition.right = _PlayerController.transform.position - transform.position;
    }

    public void Patrulha() {
        //quando estiver centralizando
        if (isCenter == false) {
            if (idTarget == 0 && transform.position.x <= wayPoints[0].transform.position.x) {
                idTarget = 1;
                StartCoroutine("UpdateTarget");
            }

            if (idTarget == 1 && transform.position.x >= wayPoints[1].transform.position.x) {
                idTarget = 0;
                StartCoroutine("UpdateTarget");
            }

            enemyRb.velocity = new Vector2((enemySpeed * direction.x) * -1, enemyRb.velocity.y);
        } else {
            enemyRb.velocity = Vector2.zero;
        }

    }
    void Atirar() {
        shotPosition.right = _PlayerController.transform.position - transform.position;
        if (isShot == false) {
            animator.SetTrigger("patada");
        }
    }

    void Shot() {
        isShot = true;

        if (isFocaArgola) {
            Rigidbody2D temp = GameObject.Instantiate(argolaPrefab, shotPosition.position, shotPosition.localRotation).GetComponent<Rigidbody2D>();
            temp.transform.localRotation = shotPosition.localRotation;
            temp.AddForce(new Vector2(0, forceY));
            temp.velocity = shotPosition.right * shotSpeed;
        } else {
            tempBall = Instantiate(shotPrefab, shotPosition.position, shotPosition.localRotation).GetComponent<Rigidbody2D>();
            tempBall.GetComponent<Rigidbody2D>().velocity = directionUpdate * forceBall;
        }
    } //instancia o tiro

    public void TakeHit() {
        enemyHP--;

        if (enemyHP <= 0) {
            animator.SetTrigger("desmaia");
            isDead = true;
            return;
        }

        StartCoroutine("Invencivel");
    } //controle da vida

    private void OnBecameInvisible() {
        matarFoca();
    }

    void matarFoca() {
        Destroy(gameObject.transform.parent.gameObject);
    }

    IEnumerator Invencivel() {
        for (int i = 0; i < changeColorTimes; i++) {
            yield return new WaitForSeconds(timeBetweenChangeColor);
            spriteRenderer.color = colorHit;
            yield return new WaitForSeconds(timeBetweenChangeColor);
            spriteRenderer.color = colorPadrao;
        }
    }

    IEnumerator UpdateTarget() //muda para um novo ponto
   {
        switch (enemyCurrentState) {
            case EnemyState.PATRULHANDO:
                isCenter = true;
                target = wayPoints[idTarget];

                yield return new WaitForSeconds(timeInPoint);
                direction = Vector3.Normalize(transform.position - target.position);
                isCenter = false;
                break;
        }
    }

    void ShotDelay() {
        Shot();
        isLockPlayer = false;
        isShot = false;
    }

    public bool isOlhandoDireita() {
        Vector3 dir = PlayerController.instance.transform.position - transform.position;

        return dir.x < 0;
    }
}
