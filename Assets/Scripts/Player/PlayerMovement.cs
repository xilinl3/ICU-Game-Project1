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
    public bool IsRight;

    public GameObject CameraFllowGo;
    private CameraFllowObject _cameraFollowObject;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  

        _cameraFollowObject = CameraFllowGo.GetComponent<CameraFllowObject>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");  
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if(Input.GetAxis("Horizontal")<0 || Input.GetAxis("Horizontal") >0)
        {
            TurnCheck();
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

    private void TurnCheck()
    {
        if(Input.GetAxis("Horizontal")>0 && !IsRight)
        {
            Trun();
        }
        else if(Input.GetAxis("Horizontal")<0 && IsRight)
        {
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

