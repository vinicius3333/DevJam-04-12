using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRena : MonoBehaviour
{
    //PRECISA DO CHIFRE BOSS RENA COMO OBJETO FILHO

    public enum EnemyState{
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
    public float shotingTime;
    public Transform arma;
    public GameObject laserPrefab;
    public bool isLookLeft;
    public bool isShotLaser;
    private GameObject temp = null;

    private bool isParado;

    // Start is called before the first frame update
    void Start()
    {
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        
        if(isLookLeft == true)
        {
            speed *= -1;
        }

        _SalaBossRena = FindObjectOfType(typeof(SalaBossRena)) as SalaBossRena;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

         switch(bossCurrentState)
        {
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
    void Update()
    {
        switch(bossCurrentState)
        {
            case EnemyState.CORRENDO:
                Run();
            break;

            case EnemyState.ATIRANDO:
                PreShot();
                break;
        }

        if(_PlayerController.gameObject.transform.position.y > bunda.transform.position.y && bunda.activeSelf == false)
        {
            bunda.SetActive(true);
        }
        else if(_PlayerController.gameObject.transform.position.y < bunda.transform.position.y && bunda.activeSelf == true)
        {
            bunda.SetActive(false);
        }
    }

    void PreShot()
    {
        if(isShotLaser == false && bossCurrentState == EnemyState.ATIRANDO)
        {
            animator.SetTrigger("Shot");
        }
    }

    public void Shot() //chamado na animacao
    {
        StartCoroutine("ShotTime");
    }

    IEnumerator ShotTime()
    {
        if(isShotLaser == false)
        {
            isShotLaser = true;
            temp =  Instantiate(laserPrefab, arma);

            if(temp != null)
            {
                temp.transform.position = arma.position;
                yield return new WaitForSeconds(shotingTime);
                temp.GetComponent<LaserColliderRena>().DestroyLaser();
                animator.SetTrigger("ShotOut");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "PlayerHit")
        {
            TakeHit();
        }
    }

    public void TakeHit() {
        enemyHP--;

        if (enemyHP < 0) {
            //MORTE
        }
    } //controle da vida

    public void Parar()
    {
        rb.velocity = Vector2.zero;
        bossCurrentState = EnemyState.PARADO;

        if(isParado == false && bossCurrentState == EnemyState.PARADO)
        {
            StopCoroutine("RandState");
            StartCoroutine("RandState");
        }
    }

    void Run()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        _SalaBossRena.Spawn();
    }

    public void Flip()
    {
        isLookLeft = !isLookLeft;
        Vector3 scale = gameObject.transform.localScale;
        transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
        speed *= -1;
        Parar();
    }

    IEnumerator RandState()
    {
        isParado = true;
        yield return new WaitForSeconds(timeInState);
        int rand = Random.Range(0, 100);
        
        if(rand > 70)
        {
            bossCurrentState = EnemyState.ATIRANDO;
        }
        else if(rand >= 20)
        {
            bossCurrentState = EnemyState.CORRENDO;
        }
        else
        {
            bossCurrentState = EnemyState.PARADO;
        }

        isParado = false;
    }
}
