using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public GameObject followObj;
    public Vector2 followOffset;
    public float speed = 3f;
    private Vector2 threshold;
    private Rigidbody2D rb;


    private void Start()
    {
        threshold = calculateThreshold();
        rb = followObj.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Vector2 follow = followObj.transform.position;
        float xDiff = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDiff = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        Vector3 newPos = transform.position;

        if (Mathf.Abs(xDiff) >= threshold.x)
        {
            newPos.x = follow.x;
        }

        if (Mathf.Abs(yDiff) >= threshold.y)
        {
            newPos.y = follow.y;
        }

        float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;
        transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
    }
    private Vector2 calculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x = followOffset.x;
        t.y = followOffset.y;
        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(followObj.transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }

}
