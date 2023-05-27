using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneHandler : MonoBehaviour
{
    public LayerMask fallStopLayers;
    public LayerMask enemyLayer;

    public float lifeTime = 5;
    public bool isRinging = false;
    public float detectionRadius;

    public float torqueForce;

    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //Set rb force

        rb.AddTorque(torqueForce);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRinging) DetectEnemiesAround();
    }

    private void DetectEnemiesAround()
    {
        Collider2D enemyFound = Physics2D.OverlapCircle(transform.position, detectionRadius, enemyLayer);
        if(enemyFound != null)
        {
            if(enemyFound.GetComponent<EnemyMovement>() != null)
                enemyFound.GetComponent<EnemyMovement>().EnterPhoneChasingState(transform.position);
        }
    }

    private void PhoneHitTheGround()
    {
        rb.Sleep();
        rb.isKinematic = true;
        isRinging = true;
        GetComponent<Animator>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((fallStopLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            PhoneHitTheGround();
        }

        if ((enemyLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.gameObject.GetComponent<EnemyMovement>() != null)
                isRinging = false;
                collision.gameObject.GetComponent<EnemyMovement>().EnterPhoneUseState();
                Destroy(gameObject);
        }
    }
}

