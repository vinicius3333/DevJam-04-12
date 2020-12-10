﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserColliderRena : MonoBehaviour
{
    public Animator animator;
    private BossRena _BossRena;
    // Start is called before the first frame update
    void Start()
    {
        _BossRena = FindObjectOfType(typeof(BossRena)) as BossRena;
        animator = GetComponent<Animator>();
        CheckFlip();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        
    }

    public void DestroyLaser() //chamei no animator
    {
        animator.SetTrigger("Destroy");
    }

    public void RealDestroy() //destroi o objecto na animacao
    {
        Destroy(this.gameObject);
        _BossRena.isShotLaser = false;
    }

    public void CheckFlip()
    {
        Vector3 scale = gameObject.transform.localScale;

        if(_BossRena.isLookLeft == true && scale.x > 0 || _BossRena.isLookLeft == false && scale.x < 0)
        {
            transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
        }
    }
}
