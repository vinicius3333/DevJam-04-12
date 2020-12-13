using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private AudioController _AudioController;
    private PlayerController _PlayerController; 
    public Transform[] wayPoints;
    public float speed;
    public float distanceToLockPlayer;
    private Transform target;
    private int idWayPoint;

    public bool isLockPlayer;
    public bool isLookLeft;
    public bool isCenter;
    private bool isPlayFX;
    void Start()
    {
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        target = wayPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(isLockPlayer == true)
        {
            target = _PlayerController.transform;
            if(isPlayFX == false)
            {
                isPlayFX = true;
                _AudioController.PlayFX(_AudioController.batAtentention, 1f);
            }
        }

        if(isCenter == true && isLockPlayer == false)
        {
            NewPos();
        }
        else if(isCenter == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        
        if(transform.position == target.position && isLockPlayer == false) { isCenter = true; }

        CheckDisPlayer();
        CheckFlip();
    }

    void NewPos()
    {
        idWayPoint = Random.Range(0, wayPoints.Length);
        target = wayPoints[idWayPoint];
        isCenter = false;
    }

    void CheckDisPlayer()
    {
        float distance = Vector3.Distance(transform.position, _PlayerController.transform.position);
        if(distance < distanceToLockPlayer) {isLockPlayer = true; isCenter = false; }
        else {isLockPlayer = false;}
    }

    void CheckFlip()
    {
        Vector3 dir = Vector3.Normalize(transform.position - target.position);

        if(dir.x > 0 && isLookLeft == false)
        {
            Flip();
        }
        else if(dir.x < 0 && isLookLeft == true)
        {
            Flip();
        }
    }

    void Flip()
    {
        isLookLeft = !isLookLeft;
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x *-1, scale.y, scale.z);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "PlayerHit")
        {
            _AudioController.PlayFX(_AudioController.batDie);
            Destroy(this.gameObject);
        }    
    }
}
