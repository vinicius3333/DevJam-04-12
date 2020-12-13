using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private PauseMenu _PauseMenu;
    private Animator animator;
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
    private bool isWalk;

    public bool moverSozinho = false;

    private Health healthClass;

    public GameController _gameController;

    private bool isAndandoGelo = false;

    public float tempoSaidaGelo;

    public float geloForce;

    public bool isPlayerParado = false;

    public float andarSozinhoForce = 0.5f;

    private GameMaster gm;

    public int maxHealth = 5;

    private Mana mana;

    // Start is called before the first frame update
    void Start() {
        _PauseMenu = FindObjectOfType(typeof(PauseMenu)) as PauseMenu;
        gm = GameObject.FindWithTag("GM").GetComponent<GameMaster>();
        if (gm.posicaoPlayer != Vector2.zero) {
            Debug.Log(gm.posicaoPlayer);
            Transform transform = GetComponent<Transform>();
            transform.position = gm.posicaoPlayer;
        }
        instance = this;
        healthClass = GetComponent<Health>();
        mana = GetComponent<Mana>();
        bounceCollider = GetComponent<BoxCollider2D>();
        playerRb = GetComponent<Rigidbody2D>();
        playerSr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (isLookLeft == true) {
            bulletSpeed *= -1;
        } else {
            bulletSpeed *= 1;
        }
    }

    // Update is called once per frame
    void Update() {
        // Verifica se o jogo tá pausado ou se o jogador tá parado e previni animações do player
        if (PauseMenu.GameIsPaused) return;

        if (isPlayerParado) {
            playerRb.velocity = Vector2.zero;
            isWalk = false;
            return;
        }

        if (moverSozinho) {
            playerRb.velocity = new Vector2(speed * andarSozinhoForce, playerRb.velocity.y);
            isWalk = true;
            return;
        }

        //Controle de Flip, para deixar o personagem olhando para o lado certo
        if (horizontal != 0) {
            isWalk = true;
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
        else
        {
            isWalk = false;
        }

        InputController();
    }
    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapArea(groundCheck[0].position, groundCheck[1].position, whatIsGround);

        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isGrounded", isGrounded);
    }

    void InputController() {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isAndandoGelo) {
            playerRb.AddForce(new Vector2(horizontal * speed * geloForce, playerRb.velocity.y));
        } else {
            playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);
        }

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
        if (isAndandoGelo) {
            playerRb.AddForce(new Vector2(0, force / 1.2f));
        } else {
            playerRb.AddForce(new Vector2(0, force));
        }
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
        if (mana.quantidadeTiro <= 0) return;
        animator.SetTrigger("Fire");
        mana.quantidadeTiro--;
        mana.atualizarQuantidade(mana.quantidadeTiro);
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
        if (gameObject.layer == 9) return;
        switch (col.gameObject.tag) {
            case "EnemyDamage":
                Debug.Log(col.gameObject.tag);
                TakeHit();
                break;
            case "Buraco":
                GameOver();
                break;
            case "ColecionavelVida":
                Destroy(col.gameObject);
                if (healthPoints < maxHealth) {
                    healthPoints++;
                    healthClass.health++;
                }
                break;
            case "ColecionavelMana":
                Destroy(col.gameObject);
                mana.quantidadeTiro += 50;
                mana.atualizarQuantidade(mana.quantidadeTiro);
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D col) {
        switch (col.gameObject.tag) {
            case "Enemy":
                TakeHit();
                break;
            case "Gelo":
                isAndandoGelo = true;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        switch (col.gameObject.tag) {
            case "Gelo":
                isAndandoGelo = false;
                break;
        }
    }

    public void Bounce() {
        playerRb.velocity = Vector2.zero;
        playerRb.AddForce(new Vector2(bounceForceX, bounceForce));
    } //controle da vida

    void TakeHit() {
        //StopAllCoroutines();
        healthPoints--;
        healthClass.health--;

        StartCoroutine("Invencivel");

        if (healthPoints <= 0) {
            GameOver();
        }
    } //controle da vida

    void GameOver() {
        Time.timeScale = 0;
        _PauseMenu.gameOverMenu.SetActive(true);
    }

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
