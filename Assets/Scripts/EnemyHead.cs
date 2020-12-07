using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    private EnemyController _EnemyController;
    void Start()
    {
        _EnemyController = FindObjectOfType(typeof(EnemyController)) as EnemyController;
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.tag == "Player")
        {
            _EnemyController.TakeHit();
        }    
    }
}
