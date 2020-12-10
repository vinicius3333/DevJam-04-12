using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRenaIce : MonoBehaviour
{
    private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }
}
