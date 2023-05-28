using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed;
    private float initialSpeed;
    [SerializeField] float _airSpeedLoss = 0f;
    Vector2 _movementDirection;
    Rigidbody2D _rb;
    Animator _anim;
    InputHolder _inputHolder;

    [Header("Jump")]
    [SerializeField] float _jumpForce = 2f;
    [SerializeField] bool _grounded;
    [SerializeField] float _inputBufferDuration;
    [SerializeField] float _coyoteTime;
    private float _coyoteTimer;
    private float _inputTimer;

    [Header("Checks")]
    [SerializeField] private Transform _headCheck;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _headCheckDistance;
    [SerializeField] Vector2 _rbVelocity;

    [Header("Miscelaneous")]
    [SerializeField] bool _activateGizmos;
    [SerializeField] Color _gizmosColor = Color.red;

    [Header("Improvements")]
    public float sidewaysWallCheckRaius = 1.0f;
    public float forwardOffset = 1.0f;
    public float checkHeight = 1.0f;
    [Range(0, 1)] public float speedReduction;

    [Header("Rotation")]
    [SerializeField] bool _facingRight = true;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputHolder = GetComponent<InputHolder>(); 
        _anim = GetComponent<Animator>();
        _coyoteTimer = _coyoteTime;
        _inputTimer = _inputBufferDuration;
        initialSpeed = _speed;
    }

    // Update is called once per frame
    void Update()
    {

        float xMov = Input.GetAxis("Horizontal");

        //Anular o Mov Se não tivermos a key available
        if (xMov > 0 && !_inputHolder.moveRight.isAvailable) xMov = 0;
        if (xMov < 0 && !_inputHolder.moveLeft.isAvailable) xMov = 0;


        GroundCheck();
        JumpInput();
        _movementDirection = new Vector3(xMov, 0).normalized;

        if (Mathf.Abs(xMov) >= 0.1f) _anim?.SetBool("isRunning", true);
        else _anim?.SetBool("isRunning", false);



        if (_coyoteTimer > 0f && _inputTimer > 0)
        {
            _anim?.SetBool("isJumping",true);
            _rb.velocity = new Vector2(_rbVelocity.x, _jumpForce);
        }

        if (Input.GetKeyUp(_inputHolder.jump.keyCode) && _rb.velocity.y > 0f)
        {
            _rb.velocity = new Vector2(_rbVelocity.x, _rbVelocity.y * 0.5f);
            _coyoteTimer = 0f;
        }

        CheckSides();

    }

    private void JumpInput()
    {
        if (_inputHolder.jump.isAvailable && Input.GetKeyDown(_inputHolder.jump.keyCode))
        {
            _inputTimer = _inputBufferDuration;
        }
        else _inputTimer -= Time.deltaTime;
    }

    private void Flip()
    {
        if (_facingRight && _movementDirection.x < 0f || !_facingRight && _movementDirection.x > 0f)
        {
            _facingRight = !_facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void FixedUpdate()
    {
        if (_grounded) _rb.velocity = new Vector3(_movementDirection.x * _speed, _rb.velocity.y);
        else _rb.velocity = new Vector3(_movementDirection.x * (_speed * (1 - _airSpeedLoss)), _rb.velocity.y);

        Flip();
    }

    void GroundCheck()
    {
        if (Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _whatIsGround))
        {
            _coyoteTimer = _coyoteTime;
            _grounded = true;
            _anim?.SetBool("isJumping", false);
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
            _grounded = false;
        }
    }

    private void CheckSides()
    {
        Vector3 checkDirection = new Vector3(_movementDirection.x, _movementDirection.y, 0);

        if (Physics2D.OverlapCircle(transform.position + Vector3.up * checkHeight + checkDirection * forwardOffset, sidewaysWallCheckRaius, _whatIsGround))
        {
            _speed = 0;
        }
        else _speed = initialSpeed;
    }


    private void OnDrawGizmos()
    {
        if (!_activateGizmos) return;

        Gizmos.color = _gizmosColor;
        Gizmos.DrawSphere(_groundCheck.position, _groundCheckRadius);
        Gizmos.DrawSphere(transform.position + Vector3.up * checkHeight, sidewaysWallCheckRaius);
    }

}