using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RigidBody2D playerRb;

    public LayerMask whatIsGround; //o que é chão

    [Header("Player Config")]
    public float healthPoints = 3;
    public float speed;
    [Range(3, 5f)]
    public float jumpForce;
    public float percDoubleJumpForce; //porcentagem da força do pulo duplo em relação ao jumpForce
    public float timeBetweenShots; //tempo entre um tiro e outro
    public Transform[] groundCheck;

    [Header("Instantiate Positions")]
    public Transform gunTrasform;

    [Header("Powers")]
    public bool isDoubleJumpActive;
    public bool isShootActive;
    public bool isShieldActive;

    [Header("Controladores")]
    private bool isGrounded;
    private bool isShoot;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<RigidBody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputController();
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapArea(groundCheck[0].position, groundCheck[1].position);
    }

    void InputController()
    {
        float time; //usado para controle do tiro
        float h = Input.GetAxisRaw("Horizontal");

        playerRb.velocity = new Vector2(h * speed, playerRb.velocity.y);

        //================ JUMP =======================
        if(Input.GetButtonDown("Jump") && isGrounded == true) //pulo normal
        {
            Jump(jumpForce);
        }

        if (Input.GetButtonDown("Jump") && isGrounded == false && isDoubleJumpActive == true) //double jump
        {
            Jump(jumpForce / percDoubleJumpForce);
        }

        if (Input.GetButtonUp("Jump") && isGrounded == false)  //zera o y quando solta o botao
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
           
        }

        //================ SHOOT ====================== (tem que testar)
        if(Input.GetButtonDown("Fire1") && isShootActive == true)
        {
            isShoot = true;
            //ATIRAR UM GAMEOBJECT NA POSIÇÃO DO GUN TRANSFORM
        }

        if(Input.GetButton("Fire1") && isShoot == true)
        {
            time = Time.deltaTime;
            if(time >= timeBetweenShots)
            {
                time = 0;
                //ATIRAR UM GAMEOBJECT
            }
        }

        if(Input.GetButtonUp("Fire1"))
        {
            time = 0;
        }


        //================ SHIELD ====================== 
        if (Input.GetButton("Fire2") && isShieldActive == true) //segura o shield
        {
            //ATIVAR O COLISOR DO SHIELD
        }

        if(Input.GetButtonUp("Fire2"))
        {
            //DESATIVA O COLISOR DO SHIELD
        }
    }

    void Jump(float force)
    {
        playerRb.AddForce(new Vector2(0, force));
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
    }
}
