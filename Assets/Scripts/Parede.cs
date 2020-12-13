using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parede : MonoBehaviour {

    public GameController _gameController;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            _gameController.proximaCena();
        }
    }

}
