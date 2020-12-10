using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanoRenaController : MonoBehaviour
{
     private BossRena _BossRena;
    // Start is called before the first frame update
    void Start()
    {
        _BossRena = FindObjectOfType(typeof(BossRena)) as BossRena;
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().Bounce();
            _BossRena.TakeHit();
        }    
    }
}
