using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNeve : MonoBehaviour {

    private new Transform transform;

    private new Rigidbody2D rigidbody;

    public Animator animator;

    public static BossNeve instance;

    public float jumpForce = 10f;

    public Transform proximaPosicao;


    private void Start() {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        instance = this;
    }

    private void Update() {
        if (isBossOlhandoDireita()) {
            transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 0));
        } else {
            transform.rotation = Quaternion.AngleAxis(180, new Vector3(0, 180, 0));
        }
    }

    public bool isBossOlhandoDireita() {
        Vector3 dir = PlayerController.instance.transform.position - transform.position;

        return dir.x > 0;
    }

    public void jogarCenoura() {
        SalaBoss.instance.jogarCenoura();
    }

    public void jogarBola() {
        SalaBoss.instance.jogarBola();
    }

    public void pularBoss() {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        animator.SetBool("onAir", true);
        StartCoroutine(mudarPosicao(proximaPosicao, 1.5f));
    }

    IEnumerator mudarPosicao(Transform _transform, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        transform.position = new Vector3(_transform.position.x, transform.position.y, 0f);
    }
}
