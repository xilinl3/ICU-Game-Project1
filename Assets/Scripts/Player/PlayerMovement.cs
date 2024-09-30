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

    private Rigidbody2D rb;
    private bool onGround;
    private bool canMove = true;
    private float remJumpCount = JUMPCOUNT;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove || isDashing)  // 如果不能移动，则不执行移动逻辑
            return;
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // 处理玩家朝向方向
        if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }


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
}