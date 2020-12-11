using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallShot : MonoBehaviour {
    public float timeToDestroy; //tempo para destruir dps que bater em algo
    public Rigidbody2D rb;
    private CircleCollider2D collider;
    public float timeToActive;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        collider.enabled = false;
        StartCoroutine("TimeToCollider");
    }
    private void OnCollisionEnter2D(Collision2D col) {
        Destroy(this.gameObject, timeToDestroy);
    }
    private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }

    IEnumerator TimeToCollider() {
        collider.enabled = false;
        yield return new WaitForSeconds(timeToActive);
        collider.enabled = true;
    }
}
