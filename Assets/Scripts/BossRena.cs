using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRena : MonoBehaviour {
    //PRECISA DO CHIFRE BOSS RENA COMO OBJETO FILHO

    public enum EnemyState {
        PARADO,
        CORRENDO,
        ATIRANDO
    }
    private PlayerController _PlayerController;
    private SalaBossRena _SalaBossRena;
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Enemy Config")]
    public EnemyState bossCurrentState; //estado atual do inimigo
    public float enemyHP;
    public GameObject bunda;
    public float timeInState; //tempo em cada ponto
    public float speed;
    public float timeToStop;
    public float shotingTime;
    public Transform arma;
    public GameObject laserPrefab;
    public bool isLookLeft;
    public bool isShotLaser;
    private GameObject temp = null;
    public bool isStartShot;
    private bool isParado;

    [Header("Dano Config")]
    public float changeColorTimes = 3;
    public float timeBetweenChangeColor = 0.3f;
    private SpriteRenderer spriteRenderer;
    public Color colorPadrao;
    public Color colorHit;
    public GameObject chifre;
    public bool tomouHit = false;

    public bool isWalk;
    public bool isDie;
    private AudioController _AudioController;
    public GameObject sacoPapaiNoel;

    public HealthBar healthBar;

    void Start() {
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        healthBar.SetMaxHealth((int)enemyHP);

        if (isLookLeft == true) {
            speed *= -1;
        }

        _SalaBossRena = FindObjectOfType(typeof(SalaBossRena)) as SalaBossRena;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        switch (bossCurrentState) {
            case EnemyState.PARADO:
                Parar();
                break;

            case EnemyState.CORRENDO:
                Run();
                break;

            case EnemyState.ATIRANDO:
                PreShot();
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        if (isDie == true) {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            StopAllCoroutines();
            chifre.SetActive(false);
            bunda.SetActive(false);
            return;
        }

        switch (bossCurrentState) {
            case EnemyState.PARADO:
                Parar();
                break;

            case EnemyState.CORRENDO:
                Run();
                break;

            case EnemyState.ATIRANDO:

                PreShot();

                break;
        }

        if (_PlayerController.gameObject.transform.position.y > bunda.transform.position.y && bunda.activeSelf == false) {
            if (tomouHit == false) {
                bunda.SetActive(true);
            } else {
                bunda.SetActive(false);
            }
        } else if (_PlayerController.gameObject.transform.position.y < bunda.transform.position.y && bunda.activeSelf == true) {
            bunda.SetActive(false);
        }

        animator.SetBool("isWalk", isWalk);
    }

    void PreShot() {
        isWalk = false;
        if (isStartShot == false && bossCurrentState == EnemyState.ATIRANDO) {
            isStartShot = true;
            animator.SetTrigger("Shot");
        }
    }

    public void Shot() //chamado na animacao
    {
        StartCoroutine("ShotTime");
    }

    IEnumerator ShotTime() {
        if (isShotLaser == false) {
            isShotLaser = true;
            temp = Instantiate(laserPrefab, arma);

            if (temp != null) {
                temp.transform.position = arma.position;
                yield return new WaitForSeconds(shotingTime);
                temp.GetComponent<LaserColliderRena>().DestroyLaser();
                animator.SetTrigger("ShotOut");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "PlayerHit" && isDie == false) {
            TakeHit();
        }
    }

    public void TakeHit() {
        tomouHit = true;
        enemyHP--;
        healthBar.SetHealth((int)enemyHP);

        if (enemyHP < 0) {
            _AudioController.ChangeMusic(_AudioController.level2);
            animator.SetTrigger("Die");
            isDie = true;
            PlayerController.instance.moverSozinho = true;
            sacoPapaiNoel.SetActive(true);
        }

        StartCoroutine("Invencivel");
    }

    public void Parar() {
        isWalk = false;
        bossCurrentState = EnemyState.PARADO;

        if (isParado == false && bossCurrentState == EnemyState.PARADO) {
            StartCoroutine("RandState");
        }
    }

    public IEnumerator TimeToZeroSpeed() //chamado pelo chifre
    {
        print("Zerou");
        rb.velocity = new Vector2(speed, rb.velocity.y);
        yield return new WaitForSeconds(timeToStop);
        rb.velocity = Vector2.zero;
    }

    void Run() {
        isWalk = true;
        rb.velocity = new Vector2(speed, rb.velocity.y);
        _SalaBossRena.Spawn();
    }

    public void Flip() {
        isLookLeft = !isLookLeft;
        Vector3 scale = gameObject.transform.localScale;
        transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
        speed *= -1;
        Parar();
    }

    IEnumerator Invencivel() {
        for (int i = 0; i < changeColorTimes; i++) {
            yield return new WaitForSeconds(timeBetweenChangeColor);
            spriteRenderer.color = colorHit;
            yield return new WaitForSeconds(timeBetweenChangeColor);
            spriteRenderer.color = colorPadrao;
        }
        tomouHit = false;
    }

    IEnumerator RandState() {
        isParado = true;
        yield return new WaitForSeconds(timeInState);
        int rand = Random.Range(0, 100);

        if (rand > 65) {
            bossCurrentState = EnemyState.ATIRANDO;
        } else {
            bossCurrentState = EnemyState.CORRENDO;
        }

        isParado = false;
    }
}
