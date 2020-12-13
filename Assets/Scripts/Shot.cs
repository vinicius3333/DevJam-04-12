using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private AudioController _AudioController;
    private SpriteRenderer sr;
    public Rigidbody2D rb;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    { 
        if(col.gameObject.layer == 8) //layer do ground
        {
            _AudioController.PlayFX(_AudioController.iceHardBroken, 1f);
            Destroy(this.gameObject, 0.002f);

        }

        switch(col.gameObject.layer)
        {
            case 11:
            col.gameObject.SendMessage("TakeHit", SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject, 0.005f);
            break;
        }
    }
}
