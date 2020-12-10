using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNeve : MonoBehaviour {

    public HealthBar healthBar;
    private new Transform transform;

    private new Rigidbody2D rigidbody;

    public Animator animator;

    public static BossNeve instance;

    public float jumpForce = 10f;

    public Transform proximaPosicao;

    public int healthPoints = 3;

    public float changeColorTimes = 3;

    public float timeBetweenChangeColor = 1.3f;

    private SpriteRenderer spriteRenderer;

    public Color colorPadrao;
    public Color colorHit;

    public bool tomouHit = false;

    public string[] estagiosBoss = { "morrendo", "terceiroEstagio", "segundoEstagio" };

    public GameObject explosao;


    private void Start() {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar.SetMaxHealth((int)healthPoints);
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

    private void OnParticleCollision(GameObject other) {
        if (!tomouHit && other.tag == "Geiser") {
            tomouHit = true;
            TakeHit();
        }
    }

    public void destruirBoss() {
        GameObject bolaNeve = GameObject.FindWithTag("BolaNeve");
        GameObject boss = GameObject.FindWithTag("BossNeve");
        if (bolaNeve != null) {
            SalaBoss.instance.destruirBolaNeve(bolaNeve);
        }
        criarParticulas();
        Destroy(boss);
    }

    void TakeHit() {
        if (healthPoints == 0) {
            return;
        }
        healthPoints--;
        healthBar.SetHealth((int)healthPoints);

        StartCoroutine("Invencivel");
    }

    IEnumerator Invencivel() {
        for (int i = 0; i < changeColorTimes; i++) {
            yield return new WaitForSeconds(timeBetweenChangeColor);
            spriteRenderer.color = colorHit;
            yield return new WaitForSeconds(timeBetweenChangeColor);
            spriteRenderer.color = colorPadrao;
        }
        //SalaBoss.instance.mudarPosicaoBossRandom();
    }

    public void proximoEstagio() {
        animator.SetTrigger(estagiosBoss[healthPoints]);
        tomouHit = false;
    }

    public void criarParticulas() {
        GameObject.Instantiate(explosao, gameObject.transform.position, gameObject.transform.rotation);
    }
}
