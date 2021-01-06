using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    Animator anim;

    public float moveSpeed, forX, upY = 3f;
    private bool facingLeft;  //flip character x direction 

    private Vector3 localScale;
    bool isWalking = true;
    bool isCrouching = true;
    bool isIdle = true;



    private void Start()
    {
        facingLeft = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
    }

    private void Update()
    {
        forX = Input.GetAxisRaw("Horizontal");
        upY = rb.velocity.y;
        rb.velocity = new Vector2(forX * moveSpeed, upY * Time.deltaTime);


        if (forX != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        anim.SetBool("isWalking", isWalking);

        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("isJumping");
        }
        else

            Idle();

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("isPunch");
        }
        else
            Idle();


        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("isKick");
        }
        else
            Idle();

        if (Input.GetButtonDown("Fire3"))
        {
            anim.SetBool("isCrouching", isCrouching);
        }
        else
            Idle();
    }
    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            if (Input.GetAxisRaw("Horizontal") > 0.5f && !facingLeft)
                Flip();
            facingLeft = true;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0.5f && facingLeft)
        {
            Flip();
            facingLeft = false;
        }

    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void Idle()
    {
        anim.SetBool("isIdle", isIdle);
    }
}


