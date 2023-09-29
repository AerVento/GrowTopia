using System;
using System.Collections;
using System.Collections.Generic;
using GrowTopia.Input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // Basic parameters
    private float _moveInputDirection = 0;

    [Header("Movements")]
    public float MoveSpeed = 10;
    public float JumpPower = 10;
    private int _nowJumpTimes = 0;
    public int MaxJumpTimes = 1;

    [Header("Ground Checks")]
    private bool _isGrounded;
    public Vector3 Offset;
    public float Radius;
    public LayerMask GroundLayer;

    // Components
    private Rigidbody2D _rb;
    private Animator _anim;

    // Input
    private InputAction _moveInputAction;
    private InputAction _jumpInputAction;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _moveInputAction = InputManager.Instance.GetAction("PlayerControl/Move/Move");
        _moveInputAction.Enable();

        _jumpInputAction = InputManager.Instance.GetAction("PlayerControl/Move/Jump");
        _jumpInputAction.Enable();
        _jumpInputAction.started += context => CheckJumpInput(); // On Jump Button Pressed
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        CheckMoveInput();
    }

    void FixedUpdate()
    {
        CheckGrounded();
        Move();
    }

    private void CheckMoveInput()
    {
        _moveInputDirection = _moveInputAction.ReadValue<float>();
    }

    private void CheckJumpInput()
    {
        if (_nowJumpTimes < MaxJumpTimes)
        { // the jump on the ground will not be counted in _nowJumpTimes
            Jump();
        }
    }

    private void CheckGrounded()
    {
        _isGrounded = _rb.velocity.y < 0.1f && Physics2D.OverlapCircle(transform.position + Offset, Radius, GroundLayer) != null;
        _anim.SetBool("IsGrounded", _isGrounded);
        if (_isGrounded)
            _nowJumpTimes = 0;
    }

    private void Move()
    {
        _rb.velocity = new Vector2(_moveInputDirection * MoveSpeed, _rb.velocity.y);
        _anim.SetFloat("HorizontalSpeed", Math.Abs(_rb.velocity.x));
        _anim.SetFloat("VerticalSpeed", Math.Abs(_rb.velocity.y));
    }

    private void Jump()
    {
        if (_isGrounded)
            transform.position += new Vector3(0, Radius, 0); // Escape from ground check
        _rb.velocity = new Vector2(_rb.velocity.x, JumpPower);
        _nowJumpTimes++;
    }

    private void Flip()
    {
        if (_moveInputDirection == 1)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (_moveInputDirection == -1)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + Offset, Radius);
    }
}
