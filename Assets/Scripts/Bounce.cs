using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {
    private PlayerController _PlayerController;

    private void Start() {
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }
    private void OnTriggerEnter2D(Collider2D col) {
        switch (col.gameObject.tag) {
            case "EnemyHead":
                //tirar dano
                _PlayerController.Bounce();
                break;
        }
    } //isso era pra tirar dano etc
}
