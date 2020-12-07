using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController _PlayerController;
    private Rigidbody2D enemyRb;

    public float enemyHP;
    public float timeInPoint;
    public float enemySpeed;
    public Transform[] wayPoints;

    public GameObject head;

    [Header("Controladores")]
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
        //checa se o player está acima da cabeça
        if(_PlayerController.gameObject.transform.position.y > head.transform.position.y && head.activeSelf == false)
        {
            head.SetActive(true);
        }
        else if(_PlayerController.gameObject.transform.position.y < head.transform.position.y && head.activeSelf == true)
        {
            head.SetActive(false);
        }

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
    
    IEnumerator UpdateTarget()
    {   
        isCenter = true;
        target = wayPoints[idTarget];

        yield return new WaitForSeconds(timeInPoint);
        direction = Vector3.Normalize(transform.position - target.position);
        isCenter = false;
    }
}
