using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed = 2f;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius;


    private Vector2 movement;
    private bool onFloor;
    private bool lookingRight = true;

    private Rigidbody2D rigidbody;
    private Animator animator;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        movement = new Vector2(horizontalInput, 0f);
        float horizontalVelocity = movement.normalized.x * speed;
        rigidbody.velocity = new Vector2(horizontalVelocity, rigidbody.velocity.y);
        onFloor = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(Input.GetButtonDown("Jump") && onFloor)
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (horizontalInput < 0f && lookingRight == true)
        {
            Flip();
        }
        else if (horizontalInput > 0f && lookingRight == false)
        {
            Flip();
        }
    }

    private void LateUpdate()
    {
        //Comprobar las animaciones
        animator.SetBool("idle", movement == Vector2.zero);
        animator.SetBool("onFloor", onFloor);
    }

    private void Flip()
    {
        lookingRight =! lookingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

    }
}
