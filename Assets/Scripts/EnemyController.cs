using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D enemyRb;

    public GameObject enemy;
    public float timeInPoint;
    public float enemySpeed;
    public Transform[] wayPoints;

    [Header("Controladores")]
    private int idTarget = 0;
    private bool isCenter = false;
    private Transform target;
    private Vector3 direction;

    private void Start()
    {
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        target = wayPoints[idTarget];

        direction = Vector3.Normalize(enemy.transform.position - target.position);
    }

    private void Update()
    {
        if(isCenter == false)
        {
            if(idTarget == 0 && enemy.transform.position.x <= wayPoints[0].transform.position.x)
            {
                idTarget = 1;
                StartCoroutine("UpdateTarget");
            }

            if(idTarget == 1 && enemy.transform.position.x >= wayPoints[1].transform.position.x)
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
    
    IEnumerator UpdateTarget()
    {   
        isCenter = true;
        target = wayPoints[idTarget];

        yield return new WaitForSeconds(timeInPoint);
        direction = Vector3.Normalize(enemy.transform.position - target.position);
        isCenter = false;
    }
}
