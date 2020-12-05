using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;

    public LayerMask whatIsGround; //o que é chão
    public bool isLookLeft; //pra onde o player está olhando na cena?

    [Header("Player Config")]
    public float healthPoints = 50;
    public float speed;
    public float jumpForce;
    public float doubleJumpForce;
    public float timeBetweenShots; //tempo entre um tiro e outro
    public Transform[] groundCheck;

    [Header("Shot Config")]
    public GameObject presentePrefab;
    public float bulletSpeed;

    [Header("Instantiate Positions")]
    public Transform gunTrasform;
    public Transform shieldPosition;

    [Header("Powers")]
    public bool isDoubleJumpActive;
    public bool isShootActive;
    public bool isShieldActive;

    [Header("Controladores")]
    private float horizontal; //eixo pegado no input
    private float time = 0; //usado para controle do tiro
    private bool isGrounded;
    private bool isShot;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Controle de Flip, para deixar o personagem olhando para o lado certo
        if(horizontal != 0)
        {
            if (horizontal > 0 && isLookLeft == true) 
            {
                Flip(); 
            }
            else if (horizontal < 0 && isLookLeft == false)
            { 
                Flip();
            }
        }

        InputController();
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapArea(groundCheck[0].position, groundCheck[1].position, whatIsGround);
    }

    void InputController()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);

        #region JUMP
        if (Input.GetButtonDown("Jump") && isGrounded == true) //pulo normal
        {
            Jump(jumpForce);
        }

        //controle da força do pulo duplo, que corrige um bug de somar forças caso aperte rapido dms
        if (Input.GetButtonDown("Jump") && isGrounded == false && isDoubleJumpActive == true && playerRb.velocity.y < 0) //double jump
        {
            Jump(doubleJumpForce);
        }

        if (Input.GetButtonDown("Jump") && isGrounded == false && isDoubleJumpActive == true && playerRb.velocity.y > 0) //double jump
        {
            Jump(doubleJumpForce / 1.2f);
        }

        if (Input.GetButtonUp("Jump"))  //diminue o y quando solta o botao e estiver subindo
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y / 2.5f);
        }

        #endregion

        #region SHOT
        if (Input.GetButtonDown("Fire1") && isShootActive == true && isShot == false)
        {
            isShot = true;
            StartCoroutine("TimeToShotAgain");
            Instantiate(presentePrefab, gunTrasform.position, transform.localRotation);
        }

        if(Input.GetButton("Fire1") && isShootActive == true) //tiro segurando o botao
        {
            time += Time.deltaTime; //controla o tempo do tiro segurando o botao

            if(time >= timeBetweenShots)
            {
                isShot = true;
                time = 0;
                Instantiate(presentePrefab, gunTrasform.position, transform.localRotation);
            }
        }

        if(Input.GetButtonUp("Fire1"))
        {
            isShot = false;
            time = 0;
        }
        #endregion

        #region SHIELD
        if (Input.GetButton("Fire2") && isShieldActive == true) //segura o shield
        {
            //ATIVAR O COLISOR DO SHIELD
        }

        if(Input.GetButtonUp("Fire2"))
        {
            //DESATIVA O COLISOR DO SHIELD
        }

        #endregion
    }

    void Jump(float force)
    {
        playerRb.AddForce(new Vector2(0, force));
    }

    void Flip()
    {
        isLookLeft = !isLookLeft;
        Vector3 scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(scale.x *-1, scale.y, scale.z);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.gameObject.tag)
        {
            case "EnemyHit":
                TakeHit();
                break;
        }
    }

    void TakeHit()
    {
        healthPoints--;

        if(healthPoints < 0)
        {
            //GAMEOVER
        }
    }

    IEnumerator TimeToShotAgain() //controla o tempo dos tiros para o jogador nao atirar rapido dms
    {
        yield return new WaitForSeconds(timeBetweenShots);
        isShot = true;
    }
}
