using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public enum EnemyType
    {
        FISH
    }

    public EnemyType enemyType;

    public float enemyHP;
    [HideInInspector]public float enemyCurrentHP;
    public float force;
    public float timeToSpawn;
    public Transform spawnPoint;
    public GameObject fishPrefab = null;
    private GameObject temp;

    void Start()
    {
        enemyCurrentHP = enemyHP;
        switch(enemyType)
        {
            case EnemyType.FISH:
            StartCoroutine("SpawnFish");
            break;
        }
    }

    IEnumerator SpawnFish()
    {
        temp = Instantiate(fishPrefab, spawnPoint);
        temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, force));
        yield return new WaitForSeconds(timeToSpawn);
        StartCoroutine("SpawnFish");
    }
}
