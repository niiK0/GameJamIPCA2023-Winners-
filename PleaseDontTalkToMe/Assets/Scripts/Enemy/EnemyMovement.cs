using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Patroling,
    Chasing,
    Returning,
    PhoneChasing,
    PhoneUse
}

public class EnemyMovement : MonoBehaviour
{
    public float patrolSpeed = 4f;
    public float chasingSpeed = 5f;
    public float returningSpeed = 5f;
    public float chasePhoneSpeed = 4f;
    public float jumpForce = 4f;

    public Vector2 detectionSize = new Vector2(7f, 1f);
    public LayerMask detectionLayerMask;
    public float moveDirection = 1f;

    public float timeToChangeDir;
    private float changeDirectionTimer;

    public Animator anim;

    private Vector3 startPosition;

    public EnemyState state = EnemyState.Patroling;

    public Transform player;

    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(transform.position, detectionSize);
    //    Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
    //}

    void Update()
    {
        switch (state)
        {
            case EnemyState.Chasing:
                if (Vector2.Distance(transform.position, player.position) <= 0.1f)
                {
                    Debug.Log("Hit the player");
                }
                break;

            case EnemyState.Patroling:
                changeDirectionTimer -= Time.deltaTime;

                if (changeDirectionTimer <= 0) ChangePatrolDirection();
                break;

            case EnemyState.Returning:
                if (Vector2.Distance(transform.position, startPosition) <= 0.1f)
                {
                    EnterPatrolState();
                }
                break;
        }

        if (moveDirection == -1) GetComponent<SpriteRenderer>().flipX = true;
        else GetComponent<SpriteRenderer>().flipX = false;

        if (Physics2D.OverlapBox(transform.position, detectionSize, 0f, detectionLayerMask) != null)
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

    private void EnterIdleState()
    {
        state = EnemyState.Idle;
        rb.velocity = new Vector2(0f, rb.velocity.y);
        //anim.SetTrigger("idle");
    }

    public void EnterReturningState()
    {
        state = EnemyState.Returning;
        moveDirection = startPosition.x < transform.position.x ? -1 : 1;
        anim.SetTrigger("return");
    }

    private void EnterPatrolState()
    {
        state = EnemyState.Patroling;
        changeDirectionTimer = timeToChangeDir;
        anim.SetTrigger("patrol");
    }

    private void EnterChasingState()
    {
        state = EnemyState.Chasing;
        moveDirection = player.position.x < transform.position.x ? -1 : 1;
        anim.SetTrigger("chasePlayer");
    }

    public void ExitPhoneChaseState()
    {
        EnterReturningState();
    }

    public void EnterPhoneChasingState(Transform phoneTransform)
    {
        if(state != EnemyState.PhoneChasing && state != EnemyState.PhoneUse)
        {
            state = EnemyState.PhoneChasing;
            moveDirection = phoneTransform.position.x < transform.position.x ? -1 : 1;
            anim.SetTrigger("chasePhone");
        }
    }

    public void EnterPhoneUseState()
    {
        state = EnemyState.PhoneUse;
        rb.velocity = new Vector2(0f, rb.velocity.y);
        anim.SetTrigger("usePhone");
        gameObject.layer = LayerMask.NameToLayer("EnemyPhone");
    }

    private void ChangePatrolDirection()
    {
        changeDirectionTimer = timeToChangeDir;
        moveDirection = -moveDirection;
    }

    private void LeavePhoneUseState()
    {
        if(state == EnemyState.PhoneUse) EnterReturningState();
        gameObject.layer = LayerMask.NameToLayer("Enemy");
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

            case EnemyState.PhoneChasing:
                rb.velocity = new Vector2(chasePhoneSpeed * moveDirection, rb.velocity.y);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform ==  player)
        {
            EnterIdleState();
            GameManager.instance.PlayerDied();  
        }
    }
}
