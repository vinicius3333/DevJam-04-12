using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalaBossRena : MonoBehaviour {
    public float qtdObjectsSpawn;
    public float timeToNextSpawn;
    public GameObject icePreafb;
    public Transform[] limiteSpawnX;
    public Transform alturaSpawn;
    private bool isInstanciado;

    public void Spawn() {
        if (isInstanciado == false) {
            StartCoroutine("SpawnIce");
        }
    }

    public IEnumerator SpawnIce() {
        isInstanciado = true;
        for(int i = 0; i <= qtdObjectsSpawn; i++)
        {
            float rand = Random.Range(limiteSpawnX[0].position.x, limiteSpawnX[1].position.x);
            Vector3 pos = new Vector3(rand, alturaSpawn.position.y, alturaSpawn.position.z);
            GameObject temp = Instantiate(icePreafb, pos, transform.rotation);

               yield return new WaitForSeconds(timeToNextSpawn);

               if(i >= qtdObjectsSpawn)
               {
                   isInstanciado = false;
               }
        }
    }
}
