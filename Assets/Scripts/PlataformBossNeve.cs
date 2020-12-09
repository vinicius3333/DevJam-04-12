using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformBossNeve : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "BossNeve") {
            SalaBoss.instance.encostarPlataforma();
        }
    }
}
