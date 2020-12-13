using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaNeve : MonoBehaviour {

    public static BolaNeve instance;
    public GameObject explosao;

    void Start() {
        instance = this;
    }

    public void criarParticulas() {
        GameObject.Instantiate(explosao, gameObject.transform.position, gameObject.transform.rotation);
    }

    public void destruirBolaNeve() {
        Destroy(gameObject);
    }
}
