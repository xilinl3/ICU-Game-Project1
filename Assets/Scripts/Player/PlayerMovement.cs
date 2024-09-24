using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //暂时用来方便测试的 可以随时删掉
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool onGround;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        if (!canMove)  // 如果不能移动，则不执行移动逻辑
            return;
        float moveInput = Input.GetAxis("Horizontal");  
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
}

