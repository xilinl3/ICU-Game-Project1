using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //暂时用来方便测试的 可以随时删掉
    public static float JUMPCOUNT = 1;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float sequentialJumpForce = 3f;
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public GameObject CameraFllowGo;

    public bool IsRight = true;
    private int facingDir = 1;

    private Rigidbody2D rb;
    private bool onGround;
    private bool canMove = true;
    private float remJumpCount = JUMPCOUNT;
    private bool isDashing = false;
    private CameraFllowObject _cameraFollowObject;
    private float _fallSpeedYDampingChangeThreshold;

    private Animator anim;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        _cameraFollowObject = CameraFllowGo.GetComponent<CameraFllowObject>();

        _fallSpeedYDampingChangeThreshold = CameraManager.Instance._fallSpeedYDampingChangeThreshold;
    }

    void Update()
    {
        if (!canMove || isDashing)  // 如果不能移动，则不执行移动逻辑
            return;
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        HandleAnimation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                remJumpCount = JUMPCOUNT;
            }
            else if (remJumpCount != 0)
            {
                remJumpCount -= 1;
                rb.AddForce(Vector2.up * sequentialJumpForce, ForceMode2D.Impulse);
            }
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }

        //if we are falling past a certain speed threshold
        if(rb.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.Instance.IsLerpingYDamping && !CameraManager.Instance.LerpedFromPlayerFalling)
        {
            CameraManager.Instance.LerpYDamping(true);
        }
        //if we are standing still or moving up
        if(rb.velocity.y >= 0 && !CameraManager.Instance.IsLerpingYDamping && CameraManager.Instance.LerpedFromPlayerFalling)
        {
            //reset so it can be called again
            CameraManager.Instance.LerpedFromPlayerFalling = false;

            CameraManager.Instance.LerpYDamping(false);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal")<0 || Input.GetAxis("Horizontal") >0)
        {
            TurnCheck();
        }
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
    }

    private void HandleAnimation()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("onGround", onGround);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    public void DisableMovement()
    {
        canMove = false;
        rb.velocity = Vector2.zero;  // 停止玩家移动
    }

    // 启用移动
    public void EnableMovement()
    {
        canMove = true;
    }

    // 角色冲刺函数
    private IEnumerator Dash()
    {
        //冲刺处理
        isDashing = true;
        Vector2 dashDirection = new Vector2(transform.localScale.x, 0).normalized;
        float dashSpeed = dashDistance / dashDuration;
        Vector2 originalVelocity = rb.velocity;
        rb.gravityScale = 0;
        rb.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);

        //冲刺结束
        rb.velocity = originalVelocity;
        rb.gravityScale = 1;
        isDashing = false;
    }
    private void TurnCheck()
    {
        if(Input.GetAxis("Horizontal")>0 && !IsRight || Input.GetAxis("Horizontal") < 0 && IsRight)
        {
            Flip();
            Trun();
        }
    }

    public void Trun()
    {
        if(IsRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsRight = !IsRight;

            //tirn the camera follow object
            _cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsRight = !IsRight;

            _cameraFollowObject.CallTurn();
        }
    }
}