using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeiserTrigger : MonoBehaviour {

    private new Transform transform;
    public GameObject Geiser;
    private new ParticleSystem particleSystem;

    private void Start() {
        transform = GetComponent<Transform>();
        particleSystem = Geiser.GetComponent<ParticleSystem>();
        particleSystem.Pause();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "BolaNeve") {
            Geiser.transform.position = transform.position;
            particleSystem.Play();
            SalaBoss.instance.destruirBolaNeve(other.gameObject);
        }
    }
}
