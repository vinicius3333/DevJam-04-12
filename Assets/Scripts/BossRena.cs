using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRena : MonoBehaviour
{
    private SalaBossRena _SalaBossRena; 
    public enum EnemyState{
        PARADO,
        CORRENDO,
        ATIRANDO
    }

    [Header("Enemy Config")]
    public EnemyState bossCurrentState; //estado atual do inimigo
    public float enemyHP;
    public float timeInPoint; //tempo em cada ponto
    public float speed;
    public float jumpForce; //forca do pulo

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        _SalaBossRena = FindObjectOfType(typeof(SalaBossRena)) as SalaBossRena;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Run()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        _SalaBossRena.Spawn();
    }

    public void Flip()
    {
        speed *= -1;
        Vector3 scale = gameObject.transform.localScale;
        transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);

    }

    public void RandState()
    {
        int rand = Random.Range(0, 100);
        if(rand < 60)
        {
            bossCurrentState = EnemyState.ATIRANDO;
        }
        else
        {
            bossCurrentState = EnemyState.CORRENDO;
        }
    }

}
