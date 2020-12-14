using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private float xMin;
    private float xMax;
    private Vector3 localScale;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        SetUpMovementBoundaries();
    }

    private void SetUpMovementBoundaries()
    {
        xMin = Camera.main.ViewportToWorldPoint(new Vector3(.5f, 0, 0)).x;
        xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

    }

    // Update is called once per frame
    void Update()
    {
        MoveLeft();
    }

    private void MoveLeft()
    {
        localScale.x = -1;
        transform.localScale = localScale;
        transform.rotation = Quaternion.Euler(0, 180, 0);

        float boundsX = Mathf.Clamp(localScale.x, xMin, xMax);
        rb.velocity = new Vector2(localScale.x * moveSpeed, rb.velocity.y);
    }
}
