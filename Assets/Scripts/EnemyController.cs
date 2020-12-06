using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject enemy;
    public float speed;
    public float timeInPoint; //tempo qnd chega num lugart definido
    public Transform[] wayPoints;

    [Header("Controladores")]
    private Transform target;
    [SerializeField]float timeTemp = 0; //controla o tempo num ponto especifico
    [SerializeField]private bool isCenter;


    // Start is called before the first frame update
    void Start()
    {
        target = wayPoints[0];
        rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, rb.velocity.y);
        UpdateVelocity();

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x == target.position.x)
        {
            SetDirection();
            isCenter = true;
        }
    }

    void SetDirection()
    {
        rb.velocity = Vector2.zero;

        timeTemp += Time.deltaTime;

        if(timeTemp >= timeInPoint)
        {
            int rand = Random.Range(0, wayPoints.Length);
            target = wayPoints[rand];

            isCenter = false;
            UpdateVelocity();
        }
    }

    void UpdateVelocity()
    {
        if (enemy.transform.position.x > target.position.x)
        {
            speed *= 1;
        }
        else if (enemy.transform.position.x < target.position.x)
        {
            speed *= -1;
        }
    }
}
