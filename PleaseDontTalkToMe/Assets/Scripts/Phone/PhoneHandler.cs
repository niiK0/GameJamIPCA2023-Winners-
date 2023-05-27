using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneHandler : MonoBehaviour
{
    public LayerMask fallStopLayers;
    public LayerMask enemyLayer;

    public float lifeTime = 5f;
    public bool isRinging = false;
    public Vector2 detectionSize = new Vector2(14f, 1f);

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
        lifeTime -= Time.deltaTime;

        Collider2D enemyFound = Physics2D.OverlapBox(transform.position, detectionSize, 0f, enemyLayer);
        if(enemyFound)
        {
            EnemyMovement enemyMovement = enemyFound.GetComponent<EnemyMovement>();
            if (enemyMovement)
            {
                enemyMovement.EnterPhoneChasingState(transform);

                if (lifeTime <= 0f)
                {
                    enemyMovement.ExitPhoneChaseState();
                }
            }

        }

        if(lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void PhoneHitTheGround()
    {
        rb.angularVelocity = 0f;
        rb.velocity = Vector2.zero;
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
            if (collision.gameObject.GetComponent<EnemyMovement>() != null) { 
                isRinging = false;
                collision.gameObject.GetComponent<EnemyMovement>().EnterPhoneUseState();
                Destroy(gameObject);
            }
        }
    }
}

