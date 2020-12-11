﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSr;
    public LayerMask whatIsGround; //o que é chão
    public bool isLookLeft; //pra onde o player está olhando na cena?

    [Header("HP Config")]
    public float healthPoints = 5;
    public float changeColorTimes; //vezes que vai trocar de cor
    public float timeBetweenChangeColor; //tempo entre piscadas

    public Color colorPadrao;
    public Color colorHit;

    [Header("Player Config")]
    public float speed;
    public float jumpForce;
    public float doubleJumpForce;
    public float bounceForce;
    public float bounceForceX;
    public float timeBetweenShots; //tempo entre um tiro e outro
    public Transform[] groundCheck;

    [Header("Powers Config")]
    public int extraJumps = 1; //quantos pulos pode dar
    public GameObject shield;
    public GameObject presentePrefab;
    public float bulletSpeed;
    private BoxCollider2D bounceCollider; //para cair no inimigo

    [Header("Positions")]
    public Transform gunTrasformX;
    public Transform gunTransformY;
    public Transform shieldTransformX;
    public Transform shieldTransformY;

    [Header("Powers")]
    public bool isDoubleJumpActive;
    public bool isShootActive;
    public bool isShieldActive;

    [Header("Controladores")]
    private float horizontal; //eixo pego no input
    private float vertical; //eixo pego no input
    private float time = 0; //usado para controle do tiro
    private bool isShot; //usado para controle de tiro
    private bool isGrounded;
    private int jumps = 0; //controla os pulos
    private bool isShield;

    public bool moverSozinho = false;

    private Health healthClass;

    // Start is called before the first frame update
    void Start() {
        instance = this;
        healthClass = GetComponent<Health>();
        bounceCollider = GetComponent<BoxCollider2D>();
        playerRb = GetComponent<Rigidbody2D>();
        playerSr = GetComponent<SpriteRenderer>();


        if (isLookLeft == true) {
            bulletSpeed *= -1;
        } else {
            bulletSpeed *= 1;
        }
    }

    // Update is called once per frame
    void Update() {
        // Verifica se o jogo tá pausado e previni animações do player
        if (PauseMenu.GameIsPaused) return;

        if (moverSozinho) {
            playerRb.velocity = new Vector2(speed / 2, playerRb.velocity.y);
            return;
        }

        //Controle de Flip, para deixar o personagem olhando para o lado certo
        if (horizontal != 0) {
            if (horizontal > 0 && isLookLeft == true) {
                Flip();
            } else if (horizontal < 0 && isLookLeft == false) {
                Flip();
            }

            //Controle da velocidade do tiro
            if (horizontal > 0 && bulletSpeed < 0) {
                bulletSpeed *= -1;
            } else if (horizontal < 0 && bulletSpeed > 0) {
                bulletSpeed *= -1;
            }
        }

        InputController();
    }
    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapArea(groundCheck[0].position, groundCheck[1].position, whatIsGround);
    }

    void InputController() {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);

        //playerRb.velocity = new Vector2(20f, playerRb.velocity.y);

        #region JUMP
        if (Input.GetButtonDown("Jump") && isGrounded == true) //pulo normal
        {
            Jump(jumpForce);
        }

        if (jumps < extraJumps) {
            //controle da força do pulo duplo, que corrige um bug de somar forças caso aperte rapido dms
            if (Input.GetButtonDown("Jump") && isGrounded == false && isDoubleJumpActive == true && playerRb.velocity.y < 0) //double jump
            {
                jumps++;
                Jump(doubleJumpForce);
            }

            if (Input.GetButtonDown("Jump") && isGrounded == false && isDoubleJumpActive == true && playerRb.velocity.y > 0) //double jump
            {
                jumps++;
                Jump(doubleJumpForce / 1.2f);
            }
        }

        if (Input.GetButtonUp("Jump"))  //diminue o y quando solta o botao e estiver subindo
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y / 2.5f);
        }

        if (isGrounded == true) {
            jumps = 0;
        }



        #endregion

        #region SHOT

        if (isShot == true) //Controle do tempo entre tiros
        {
            time += Time.deltaTime;

            if (time > timeBetweenShots) {
                isShot = false;
                time = 0;
            }
        }

        if (isShield == false && isShootActive == true && isShot == false) {
            if (Input.GetButtonDown("Fire1")) {
                SetShotPosition();
            } else if (Input.GetButton("Fire1")) //tiro segurando o botao
              {
                SetShotPosition();
            }
        }

        #endregion

        #region SHIELD
        if (Input.GetButton("Fire2") && isShieldActive == true) //segura o shield
        {
            isShield = true;
            shield.SetActive(true);

            if (horizontal == 0 || horizontal != 0) {
                SetShieldPosition(shieldTransformX);
            }

            if (vertical != 0) {
                SetShieldPosition(shieldTransformY);
            }
        }

        if (Input.GetButtonUp("Fire2")) {
            isShield = false;
            shield.SetActive(false);
        }

        #endregion
    }

    void Jump(float force) {
        playerRb.AddForce(new Vector2(0, force));
    }

    void Flip() {
        isLookLeft = !isLookLeft;
        Vector3 scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
    } //flipa o objeto

    void SetShotPosition() {
        bool isX = false;
        Transform temp = null;

        if (horizontal == 0 || horizontal != 0) {
            temp = gunTrasformX;
            isX = true;
        }

        if (vertical != 0) {
            temp = gunTransformY;
            isX = false;
        }

        Shot(temp, isX);
    } //controla a posicao do tiro antes de instanciar

    void Shot(Transform newTransform, bool eixoX) {
        isShot = true;
        Rigidbody2D temp = Instantiate(presentePrefab, newTransform.position, transform.localRotation).GetComponent<Rigidbody2D>();

        if (eixoX == true) {
            temp.velocity = new Vector2(bulletSpeed, 0);
        } else {
            if (bulletSpeed < 0) { bulletSpeed *= -1; }
            temp.velocity = new Vector2(0, bulletSpeed);
        }

    } //instancia o tiro

    void SetShieldPosition(Transform newPosition) {
        shield.transform.position = newPosition.position;
        shield.transform.rotation = newPosition.rotation;
    } //controla a posicao do shield

    private void OnTriggerEnter2D(Collider2D col) {
        switch (col.gameObject.tag) {
            case "EnemyDamage":
                TakeHit();
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D col) {
        switch (col.gameObject.tag) {
            case "Enemy":
                TakeHit();
                break;
            case "ColecionavelVida":
                Destroy(col.gameObject);
                healthClass.health++;
                break;
        }
    }

    public void Bounce() {
        playerRb.velocity = Vector2.zero;
        playerRb.AddForce(new Vector2(bounceForceX, bounceForce));
    } //controle da vida

    void TakeHit() {
        Debug.Log("teste");
        healthPoints--;
        healthClass.health--;

        StartCoroutine("Invencivel");

        if (healthPoints < 0) {
            GetComponent<GameController>().resetarCena();
        }
    } //controle da vida

    IEnumerator Invencivel() {
        gameObject.layer = 9; //invencivel layer

        for (int i = 0; i < changeColorTimes; i++) {
            yield return new WaitForSeconds(timeBetweenChangeColor);
            playerSr.color = colorHit;
            yield return new WaitForSeconds(timeBetweenChangeColor);
            playerSr.color = colorPadrao;
        }

        gameObject.layer = 10; //player layer
    }

}
