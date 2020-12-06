using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private SpriteRenderer sr;
    public Rigidbody2D rb;
    public Color[] spritePresente;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        int i = Random.Range(0, spritePresente.Length);

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
