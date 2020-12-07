using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private SpawnController _SpawnController;
    private Rigidbody2D rb;
    private Vector2 scale;
    
    private void Start()
    {
        _SpawnController = GetComponentInParent<SpawnController>();
        rb = GetComponent<Rigidbody2D>();
        scale = transform.localScale;
    }

    private void Update()
    {
        if(rb.velocity.y < -0.5f && scale.y > 0)
        {            
            transform.localScale = new Vector2(scale.x, scale.y *-1);
        }
    }

    private void OnBecameInvisible() 
    {
        Destroy(this.gameObject);
    }
    
    public void TakeHit() {
        _SpawnController.enemyCurrentHP--;

        if (_SpawnController.enemyCurrentHP <= 0) {
            Destroy(this.gameObject);
            _SpawnController.enemyCurrentHP = _SpawnController.enemyHP; 
        }
    } //controle da vida
}