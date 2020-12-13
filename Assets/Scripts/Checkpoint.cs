using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    private Animator animator;

    public Transform posicaoReset;

    private GameMaster gm;

    private bool isUsed = false;

    private void Start() {
        animator = GetComponent<Animator>();
        gm = GameObject.FindWithTag("GM").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && !isUsed) {
            isUsed = true;
            animator.SetTrigger("checkpoint");
            gm.CheckpointPlayer(posicaoReset.position);
        }
    }
}
