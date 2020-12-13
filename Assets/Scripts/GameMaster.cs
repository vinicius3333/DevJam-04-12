using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    private static GameMaster instance;
    public Vector2 posicaoPlayer;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        } else {
            Destroy(gameObject);
        }
    }

    public void CheckpointPlayer(Vector2 posicao) {
        posicaoPlayer = posicao;
    }
}
