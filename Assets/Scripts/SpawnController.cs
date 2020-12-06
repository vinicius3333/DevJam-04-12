using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public float force;
    public Transform spawnPoint;
    public GameObject fishPrefab = null;
    private GameObject temp;
    private bool isIstantiate;

    void Start()
    {
        temp = Instantiate(fishPrefab, spawnPoint);
        temp.SendMessage("StartForce", force, SendMessageOptions.DontRequireReceiver);
        isIstantiate = true;
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
