﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    PATRULHANDO,
    PERSEGUINDO,
    VOLTANDO
}

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private PlayerController _PlayerController;
    private Rigidbody2D enemyRb;

    [Header("Enemy Config")]
    public EnemyState enemyCurrentState; //estado atual do inimigo
    public float enemyHP;
    public float timeInPoint; //tempo em cada ponto
    public float enemySpeed;
    public float jumpForce; //forca do pulo
    public Transform[] wayPoints; //pontons para percorrer, e tambem usado para definir a area que ele proteje
    public Transform[] groundCheck; //groundCheck

    [Header("Perseguindo")]
    public float hunterTime; //tempo de perseguição até voltar a origem

    [Header("Raycast Config")]
    public float rayDistance; //distancia maxima que o raio atinge
    private RaycastHit2D rayHit; //o que o raio bate
    public LayerMask rayLayerGround; //usado para checar o chao, e tambem para testar se ele pode pular
    public LayerMask whatIsPlayer; //saber se o player entrou na area de proteção do inimigo

    public float returnDistance; //quando ele volta para a origem tem q saber a distancia dele para o ponto, para mudar o estad

    [Header("Dano Config")]
    public float changeColorTimes = 3;
    public float timeBetweenChangeColor = 0.3f;
    private SpriteRenderer spriteRenderer;
    public Color colorPadrao;
    public Color colorHit;
    
    [Header("Controladores")]
    private bool isGrounded;
    private bool isJump;
    private bool isPlayerDetected; //se o player entrou na area de colisao
    private int idTarget = 0; //controla o objetivo
    private bool isCenter = false; //controla se está centralizado no ponto objetivo
    private Transform target; //objetivo
    private Vector3 direction; //direcao do movimento setado da posAtual para o objetivp
    private bool isWalk;
    private bool isDie;
    public bool isLookLeft;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        target = wayPoints[idTarget];

        direction = Vector3.Normalize(transform.position - target.position);
    }

    private void Update()
    {
        if(isDie == true) 
        {
            enemyRb.velocity = Vector2.zero;
            enemyRb.gravityScale = 0;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            StopAllCoroutines();
            return;
        }

        switch(enemyCurrentState)
        {
            case EnemyState.PATRULHANDO:
                Patrulha();
            break;

            case EnemyState.PERSEGUINDO:
                Perseguir();
            break;

            case EnemyState.VOLTANDO:
                Voltando();
                break;
        }
        CheckFlip();
    }

    private void FixedUpdate() {
        
        if(isDie == true) 
        {
            return;
        }

        //debugar o raycast
        //Debug.DrawRay(transform.position, direction *-1 * 1.5f, Color.red);

        //detecco do player
        isPlayerDetected = Physics2D.OverlapArea(wayPoints[0].position, wayPoints[1].position, whatIsPlayer);
        //deteccao do chao
        isGrounded = Physics2D.OverlapArea(groundCheck[0].position, groundCheck[1].position, rayLayerGround);

        animator.SetBool("isWalk", isWalk);
    }

    public void Patrulha()
    {
        TargetController();

        //se o jogador entrar na area de guarda o estado muda para perseguicao;
        if(isPlayerDetected == true && enemyCurrentState == EnemyState.PATRULHANDO)
        {
            enemyCurrentState = EnemyState.PERSEGUINDO;
        }
    }

    public void Perseguir()
    {
        //se o player sair da area de guarda começa a contagem de para de seguir
        if(isPlayerDetected == false)
        {
            StartCoroutine("HunterTiming");
        }
        else //se continuar dentro não conta
        {
            StopCoroutine("HunterTiming");
        }

        target = _PlayerController.transform; //seta o objetivo para o player
        direction = Vector3.Normalize(transform.position - target.position); //seta a direcao para o player

        //testa se tem um ground na sua frente qnd esta em perseguição
        rayHit = Physics2D.Raycast(transform.position, new Vector2(direction.x *-1, 0), rayDistance, rayLayerGround);
        
        //faz pular
        if(rayHit == true && isGrounded == true && isJump == false)
        {
            Jump();
        }
        else if(isGrounded == true && isJump == true)
        {
            isJump = false;
        }
        
        
        if(isJump == false)
        {
            isWalk = true;
        }

        //movimento
        enemyRb.velocity = new Vector2((enemySpeed * direction.x)*-1, enemyRb.velocity.y);
    }

    public void Voltando()
    {
        //testa a posicao do player para idicar o waypoint mais proximo
        if(_PlayerController.transform.position.x <  wayPoints[0].transform.position.x)
        {
            idTarget = 0;
        }
        else if(_PlayerController.transform.position.x >  wayPoints[1].transform.position.x)
        {
            idTarget = 1;
        }

        if(isJump == false)
        {
            isWalk = true;
        }

        target = wayPoints[idTarget];
        direction = Vector3.Normalize(transform.position - target.position);
        enemyRb.velocity = new Vector2((enemySpeed * direction.x)*-1, enemyRb.velocity.y);

        float distance = Vector3.Distance(transform.position, target.position);

        //testa se a distancia é suficiente para mudar de estado
        if(distance < returnDistance)
        {
            enemyCurrentState = EnemyState.PATRULHANDO;
        }

        //testa se pode pular
        rayHit = Physics2D.Raycast(transform.position, new Vector2(direction.x *-1, 0), rayDistance, rayLayerGround);
        
        if(rayHit == true && isGrounded == true && isJump == false)
        {
            Jump();
        }
        else if(isGrounded == true && isJump == true)
        {
            isJump = false;
        }

    }

    void Jump() //jump kk
    {
        isWalk = false;
        isJump = true;
        enemyRb.AddForce(new Vector2(direction.x *-1, jumpForce));
    }

    void TargetController() //controla o objetivo e movimento quando em PATRULHA
    {
        //quando estiver centralizando
        if(isCenter == false)
        {
            isWalk = true;
            if(idTarget == 0 && transform.position.x <= wayPoints[0].transform.position.x)
            {
                idTarget = 1;
                StartCoroutine("UpdateTarget");
            }

            if(idTarget == 1 && transform.position.x >= wayPoints[1].transform.position.x)
            {   
                idTarget = 0;
                StartCoroutine("UpdateTarget");
            }

            enemyRb.velocity = new Vector2((enemySpeed * direction.x)*-1, enemyRb.velocity.y);
        }
        else
        {
            enemyRb.velocity = Vector2.zero;
            isWalk = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "PlayerHit" && isDie == false)
        {
            TakeHit();
        }
    }

    public void TakeHit() {
        enemyHP--;
        StartCoroutine("Invencivel");
        if (enemyHP <= 0) {
            animator.SetTrigger("die");
            isDie = true;
        }
    } //controle da vida
    
    IEnumerator HunterTiming()
    {
        yield return new WaitForSeconds(hunterTime);
        enemyCurrentState = EnemyState.VOLTANDO;
    }

    IEnumerator UpdateTarget() //muda para um novo ponto
    {   
        switch(enemyCurrentState)
        {
            case EnemyState.PATRULHANDO:
                isCenter = true;
                target = wayPoints[idTarget];

                yield return new WaitForSeconds(timeInPoint);
                direction = Vector3.Normalize(transform.position - target.position);
                isCenter = false;
            break;
        }

    }
    IEnumerator Invencivel()
    {
        for (int i = 0; i < changeColorTimes; i++) {
            yield return new WaitForSeconds(timeBetweenChangeColor);
            spriteRenderer.color = colorHit;
            yield return new WaitForSeconds(timeBetweenChangeColor);
            spriteRenderer.color = colorPadrao;
        }
    }
    void CheckFlip()
    {
        Vector3 dir = Vector3.Normalize(transform.position - target.position);

        if(dir.x > 0 && isLookLeft == false)
        {
            Flip();
        }
        else if(dir.x < 0 && isLookLeft == true)
        {
            Flip();
        }
    }

    void Flip()
    {
        isLookLeft = !isLookLeft;
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x *-1, scale.y, scale.z);
    }
}
