using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{

    Rigidbody2D rb2D;

    [Header(" Projectile Attributes")]
    public float projectileSpeed = 15f;
    [SerializeField] private int projectileDamage = 25;
    private SpriteRenderer sr;
    bool isRightPosition = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        isRightPosition = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CorrectOrientation();
    }

    private void CorrectOrientation()
    {
        isRightPosition = transform.position.x < Player.instance.transform.position.x;

        if (isRightPosition)
        {
            sr.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        //sr.transform.rotation = new Quaternion(0, 180, 0, 0);
        rb2D.velocity = new Vector2(projectileSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().ReceiveDamage(projectileDamage);
        }
        else
        {
            return;
        }

        this.gameObject.SetActive(false);
    }
}
