using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoPadrao : MonoBehaviour {
    private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }
}
