using System.Collections;
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
    private PlayerController _PlayerController;
    private Rigidbody2D enemyRb;

    [Header("Enemy Config")]
    public EnemyState enemyCurrentState;
    public float enemyHP;
    public float timeInPoint;
    public float enemySpeed;
    public float jumpForce;
    public Transform[] wayPoints;
    public Transform[] groundCheck;

    [Header("Perseguindo")]
    public float hunterTime;

    [Header("Raycast Config")]
    public float rayDistance;
    private RaycastHit2D rayHit;
    public LayerMask rayLayerGround;
    public LayerMask whatIsPlayer;

    public float returnDistance;
    public GameObject head;

    [Header("Controladores")]
    private bool isGrounded;
    private bool isJump;
    private bool isPlayerDetected;
    private int idTarget = 0;
    private bool isCenter = false;
    private Transform target;
    private Vector3 direction;

    private void Start()
    {
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        target = wayPoints[idTarget];

        direction = Vector3.Normalize(transform.position - target.position);
    }

    private void Update()
    {
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

        //checa se o player está acima da cabeça
        if(_PlayerController.gameObject.transform.position.y > head.transform.position.y && head.activeSelf == false)
        {
            head.SetActive(true);
        }
        else if(_PlayerController.gameObject.transform.position.y < head.transform.position.y && head.activeSelf == true)
        {
            head.SetActive(false);
        }
    }

    private void FixedUpdate() {
        Debug.DrawRay(transform.position, direction *-1 * 1.5f, Color.red);

        //detecco do player
        isPlayerDetected = Physics2D.OverlapArea(wayPoints[0].position, wayPoints[1].position, whatIsPlayer);
        //deteccao do chao
        isGrounded = Physics2D.OverlapArea(groundCheck[0].position, groundCheck[1].position, rayLayerGround);
    }

    public void Patrulha()
    {
        TargetController();

        if(isPlayerDetected == true && enemyCurrentState == EnemyState.PATRULHANDO)
        {
            enemyCurrentState = EnemyState.PERSEGUINDO;
        }
    }

    public void Perseguir()
    {
        if(isPlayerDetected == false)
        {
            StartCoroutine("HunterTiming");
        }
        else
        {
            StopCoroutine("HunterTiming");
        }

        target = _PlayerController.transform;
        direction = Vector3.Normalize(transform.position - target.position);
        rayHit = Physics2D.Raycast(transform.position, new Vector2(direction.x *-1, 0), rayDistance, rayLayerGround);
        
        if(rayHit == true && isGrounded == true && isJump == false)
        {
            Jump();
        }
        else if(isGrounded == true && isJump == true)
        {
            isJump = false;
        }
        
        enemyRb.velocity = new Vector2((enemySpeed * direction.x)*-1, enemyRb.velocity.y);
    }

    public void Voltando()
    {
        if(_PlayerController.transform.position.x <  wayPoints[0].transform.position.x)
        {
            idTarget = 0;
        }
        else if(_PlayerController.transform.position.x >  wayPoints[1].transform.position.x)
        {
            idTarget = 1;
        }

        target = wayPoints[idTarget];
        direction = Vector3.Normalize(transform.position - target.position);
        enemyRb.velocity = new Vector2((enemySpeed * direction.x)*-1, enemyRb.velocity.y);

        float distance = Vector3.Distance(transform.position, target.position);

        if(distance < returnDistance)
        {
            enemyCurrentState = EnemyState.PATRULHANDO;
        }

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

    void Jump()
    {
        isJump = true;
        enemyRb.AddForce(new Vector2(direction.x *-1, jumpForce));
    }

    void TargetController()
    {
        //quando estiver centralizando
        if(isCenter == false)
        {
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
        }
    }

    public void TakeHit() {
        enemyHP--;

        if (enemyHP <= 0) {
            Destroy(gameObject.transform.parent.gameObject);
        }
    } //controle da vida
    
    IEnumerator HunterTiming()
    {
        yield return new WaitForSeconds(hunterTime);
        enemyCurrentState = EnemyState.VOLTANDO;
    }

    IEnumerator UpdateTarget()
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
}
