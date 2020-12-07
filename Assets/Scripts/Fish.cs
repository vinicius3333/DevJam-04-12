using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 scale;
    
    private void Start()
    {
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
}