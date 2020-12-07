using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public float force;
    public float timeToSpawn;
    public Transform spawnPoint;
    public GameObject fishPrefab = null;
    private GameObject temp;

    void Start()
    {
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        temp = Instantiate(fishPrefab, spawnPoint);
        temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, force));
        yield return new WaitForSeconds(timeToSpawn);
        StartCoroutine("Spawn");
    }
}
