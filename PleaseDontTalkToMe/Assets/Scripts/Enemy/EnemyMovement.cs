using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Patroling,
    Chasing,
    Returning,
    ChasingPhone,
    PhoneUse
}

public class EnemyMovement : MonoBehaviour
{
    public float patrolSpeed = 4f;
    public float chasingSpeed = 5f;
    public float returningSpeed = 5f;
    public float chasePhoneSpeed = 4f;
    public float jumpForce = 4f;

    public float detectionRadius = 2.5f;
    public LayerMask detectionLayerMask;
    public float moveDirection = 1;

    public float timeToChangeDir;
    private float changeDirectionTimer;

    private Vector3 startPosition;

    public EnemyState state = EnemyState.Patroling;

    public Transform player;

    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
    }

    void Update()
    {
        switch (state)
        {
            case EnemyState.Chasing:
                changeDirectionTimer -= Time.deltaTime;

                if (changeDirectionTimer <= 0) ChangePatrolDirection();
                break;

            case EnemyState.Patroling:
                rb.velocity = new Vector2(patrolSpeed * moveDirection, rb.velocity.y);
                break;

            case EnemyState.Returning:
                if (Vector2.Distance(transform.position, startPosition) <= 0.1f)
                {
                    EnterPatrolState();
                }
                break;

            case EnemyState.ChasingPhone:
                if (Vector2.Distance(transform.position, startPosition) <= 0.1f)
                {
                    EnterPatrolState();
                }
                break;
        }

        if (Physics2D.OverlapCircle(transform.position, detectionRadius, detectionLayerMask) != null)
        {
            if (state == EnemyState.Patroling || state == EnemyState.Returning)
            {
                EnterChasingState();
            }

        }
        else if(state == EnemyState.Chasing)
        {
            EnterReturningState();
        }
    }

    private void EnterReturningState()
    {
        state = EnemyState.Returning;
        moveDirection = startPosition.x < transform.position.x ? -1 : 1;
    }

    private void EnterPatrolState()
    {
        state = EnemyState.Patroling;
        changeDirectionTimer = 0f;
    }

    private void EnterChasingState()
    {
        state = EnemyState.Chasing;
    }

    public void EnterChasingPhoneState(Vector2 phonePosition)
    {
        state = EnemyState.ChasingPhone;
        moveDirection = phonePosition.x < transform.position.x ? -1 : 1;
    }

    public void EnterPhoneUseState(Vector2 phonePosition)
    {
        state = EnemyState.PhoneUse;
        //Do phone animation
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1, 0, 0, 0.3f);
    //    Gizmos.DrawSphere(transform.position, detectionRadius);
    //}

    private void ChangePatrolDirection()
    {
        changeDirectionTimer = timeToChangeDir;
        moveDirection = -moveDirection;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case EnemyState.Chasing:
                rb.velocity = new Vector2(chasingSpeed * moveDirection, rb.velocity.y);
                break;

            case EnemyState.Patroling:
                rb.velocity = new Vector2(patrolSpeed * moveDirection, rb.velocity.y);
                break;

            case EnemyState.Returning:
                rb.velocity = new Vector2(returningSpeed * moveDirection, rb.velocity.y);
                break;

            case EnemyState.ChasingPhone:
                rb.velocity = new Vector2(chasePhoneSpeed * moveDirection, rb.velocity.y);
                break;
            case EnemyState.PhoneUse:
                rb.velocity = new Vector2(0f, rb.velocity.y);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player) Destroy(this);
    }
}
