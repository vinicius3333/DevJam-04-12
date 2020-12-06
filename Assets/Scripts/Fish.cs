using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Update()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if(rb.velocity.y < 0)
        {
            float scale = transform.localScale.y;

            transform.localScale = new Vector2(0, scale * -1);
        }
    }

    public void StartForce(float force)
    {
        rb.velocity = new Vector2(0, force);
    }
}
