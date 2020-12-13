using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallShot : MonoBehaviour {
    private AudioController _AudioController;
    public float timeToDestroy; //tempo para destruir dps que bater em algo
    public Rigidbody2D rb;
    private new CircleCollider2D collider;
    public float timeToActive;


    private void Start() {
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        collider.enabled = true;
    }
    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            Destroy(this.gameObject, timeToDestroy);
        }
        _AudioController.PlayFX(_AudioController.ballBounce, 1f);
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
