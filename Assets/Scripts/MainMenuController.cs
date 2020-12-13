using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {
    public new Transform transform;
    void Start() {
        transform = GetComponent<Transform>();
    }

    void Update() {
        transform.position += Vector3.right * 0.01f;
    }
}
