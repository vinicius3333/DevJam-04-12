using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallShot : MonoBehaviour
{
    public float timeToDestroy; //tempo para destruir dps que bater em algo
    public Rigidbody2D rb;
    // Start is called before the first frame update
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D col) 
    {
        Destroy(this.gameObject, timeToDestroy);
    }
        private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
