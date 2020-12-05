using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private PlayerController _PlayerController;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    public Color[] spritePresente;

    private void Start()
    {
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        int i = Random.Range(0, spritePresente.Length);

        rb.velocity = new Vector2(_PlayerController.bulletSpeed, 0);
        sr.color = spritePresente[i];
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    { 
        if(col.gameObject.layer.ToString() == "Ground")
        {
            Destroy(this.gameObject);
        }
    }
}
