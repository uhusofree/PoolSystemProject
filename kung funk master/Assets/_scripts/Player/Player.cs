using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    #region VARIABLES
    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool facingRight = true;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator anim;
    public LayerMask groundLayer;
    public BoxCollider2D box2d;
    private Vector2 orginalSize;
    private Vector2 originalOffset;


    [Header("Physics")]
    public float maxSpeed;
    public float linearDrag;
    public float jumpSpeed;
    public float jumpDelay = .25f;
    private float jumpTimer;
    public float gravity = 1;
    public float gravityMultiplier = 1;

    [Header("Punch Details")]
    public float punchDelay = .25f;
    private float punchTimer;
    public bool punch = false;
    public Transform punchPoint;
    public float punchRadius = .5f;
    public int punchDamage = 25;
    [Header("Kick Details")]
    public float kickDelay = .25f;
    private float kickTimer;
    public bool kick = false;
    public Transform kickPoint;
    public float kickRadius = .25f;
    public int kickDamage = 25;
    [Header("Crouch Kick Details")]
    public Transform crouchKickPoint;
    public float crouchKickRadius = .25f;
    public int crouchKickDamage = 25;
    public LayerMask enemyLayers;

    [Header("Collision")]
    public bool isGrounded;
    public float rayLength = .6f;
    bool isCrouched = false;

    [Header("Player Health")]
    public int maxHealth = 200;
    public int health;
    private int hurting = Animator.StringToHash("isHurting");
    public HealthBar healthBarUI;

    [SerializeField] private float invulnerableTimer;
    [SerializeField] private float resetInvulnerableTimer = 20f;
    private bool isInvulnerable = false;
    public InvulnerableBar invulnerableBar;
    public GameObject invulernablePopUp;
    [SerializeField] private float popUpTimer;
    [SerializeField] private AudioClip stimulus;
    [SerializeField] [Range(0, 1)] private float sfxVolume = .3f;
    #endregion

    private static Player _instance;

    public static Player instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        box2d = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        orginalSize = new Vector2(1.1f, 4.08f);
        originalOffset = new Vector2(0, .33f);
        health = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);
    }

    #region PLAYER INPUT 
    private void Update()
    {

        InvulnerabilityCountdown();

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);

        if (!isGrounded)
        {
            box2d.size = new Vector2(box2d.size.x, 1.75f);
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;

        }

        if (Input.GetButtonDown("CrouchKick"))
        {

            anim.SetBool("isCrouchKick", true);
            box2d.size = new Vector2(box2d.size.x, box2d.size.y / 2);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(crouchKickPoint.position, crouchKickRadius, enemyLayers);

            foreach (Collider2D hit in hitEnemies)
            {
                hit.GetComponent<Health>().TakeDamage(crouchKickDamage);
            }
        }
        else if (Input.GetButtonUp("CrouchKick"))
        {
            box2d.size = orginalSize;
            anim.SetBool("isCrouchKick", false);
            rb.constraints = RigidbodyConstraints2D.None
                ;
        }

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //Debug.Log(direction);

        if (Input.GetButtonDown("Punch"))
        {
            if (isCrouched == true)
            {
                return;
            }
            else
            {
                punchTimer = Time.time + punchDelay;
                punch = true;

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(punchPoint.position, punchRadius, enemyLayers);

                foreach (Collider2D hit in hitEnemies)
                {
                    hit.GetComponent<Health>().TakeDamage(punchDamage);
                    //Debug.Log("Health is " + hit.GetComponent<Health>().currentHealth);
                }
            }


        }

        if (Input.GetButtonDown("Kick"))
        {
            if (isCrouched == true)
            {
                return;
            }

            else
            {
                kickTimer = Time.time + kickDelay;
                kick = true;
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(kickPoint.position, kickRadius, enemyLayers);

                foreach (Collider2D hit in hitEnemies)
                {
                    hit.GetComponent<Health>().TakeDamage(kickDamage);
                    //Debug.Log("Health is " + hit.GetComponent<Health>().currentHealth);
                }
            }
        }


        if (BossAttacks.instance)
        {
            if (BossAttacks.instance.isBellyBashing == true)
            {
                Vector2 pushBack = BossAttacks.instance.transform.position - transform.position;

                pushBack = -pushBack.normalized;
                rb.AddForce(pushBack * 1500);
            }
            else
            {
                return;
            }
        }

    }
    #endregion
    private void FixedUpdate()
    {
        MoveCharacter(direction.x);
        ModifyingPhysics();
        StartJump();
        StartPunch();
        StartKick();
        StartCrouch();
    }
    #region MOVEMENT
    void MoveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * Time.deltaTime * moveSpeed);


        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

        anim.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
    }

    void StartCrouch()
    {
        if (direction.y < 0)
        {
            isCrouched = true;
            box2d.size = new Vector2(box2d.size.x, 1.75f);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            anim.SetFloat("vertical", direction.y);
        }
        else if (direction.y >= 0)
        {
            isCrouched = false;
            rb.constraints = RigidbodyConstraints2D.None;
            box2d.size = orginalSize;
            anim.SetFloat("vertical", direction.y);
        }
    }

    void Jump()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

        anim.SetBool("isJumping", true);


        jumpTimer = 0;
    }

    void StartJump()
    {
        if (jumpTimer > Time.time && isGrounded)
        {
            Jump();
        }
        else if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
    }
    #endregion

    #region PLAYER ATTACK ANIMATION
    void StartPunch()
    {
        if (punchTimer > Time.time)
        {
            anim.SetBool("isPunching", true);

        }
        else if (punch)
        {
            anim.SetBool("isPunching", false);
        }
    }

    void StartKick()
    {

        if (kickTimer > Time.time)
        {
            anim.SetBool("isKicking", true);

        }
        else if (kick)
        {
            anim.SetBool("isKicking", false);
        }
    }


    #endregion

    #region PLAYER PHYSICS 
    void ModifyingPhysics()
    {
        bool changingDir = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);


        if (isGrounded)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDir)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 30f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * gravityMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (gravityMultiplier / 2);
            }
        }

    }
    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(punchPoint.position, punchRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(kickPoint.position, kickRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(crouchKickPoint.position, kickRadius);

    }

    #region PLAYER HEALTH
    public void ReceiveDamage(int damage)
    {
        if (isInvulnerable == false)
        {
            StartCoroutine(Invulnerable());
            health -= damage;

            if (health <= maxHealth / 2 || health <= maxHealth / 3)
            {
                CameraShake.instance.ShakeCamera();
            }
            healthBarUI.SetHealthBar(health, maxHealth);

            Debug.Log("Current Health is" + health);

            if (health <= 0)
            {
                Die();
            }
        }

    }

    private IEnumerator Invulnerable()
    {

        box2d.enabled = false;
        anim.SetTrigger(hurting);
        Input.ResetInputAxes();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        yield return new WaitForSeconds(.125f);
        box2d.enabled = true;
        anim.SetBool("isIdle", true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HealthBoost healthBoost = other.gameObject.GetComponent<HealthBoost>();
        Invulnerability invulnerabilityBoost = other.GetComponent<Invulnerability>();
        InteractWithPickUp(healthBoost, invulnerabilityBoost);
    }

    private void InteractWithPickUp(HealthBoost healthBoost, Invulnerability invulnerabilityBoost)
    {
        if (healthBoost)
        {
            InteractWithHealthItem(healthBoost);
        }

        else if (invulnerabilityBoost)
        {
            InteractWithInvulnerabilityItem(invulnerabilityBoost);
        }
    }

    private void InteractWithInvulnerabilityItem(Invulnerability invulnerabilityBoost)
    {
        isInvulnerable = true;
        invulnerabilityBoost.OnHit();
        invulernablePopUp.SetActive(true);
        AudioSource.PlayClipAtPoint(stimulus, Camera.main.transform.position, sfxVolume);
        invulnerableBar.SetInvulnerabilityMaxValue(resetInvulnerableTimer);
    }

    private void InvulnerabilityCountdown()
    {

        if (isInvulnerable)
        {
            invulnerableTimer -= Time.deltaTime;
            invulnerableBar.SetInvulernabilityBarValue(invulnerableTimer);
            if (invulnerableTimer <= 17f)
            {
                invulernablePopUp.SetActive(false);
            }
        }
        if (invulnerableTimer <= 0)
        {
            invulnerableTimer = resetInvulnerableTimer;
            isInvulnerable = false;
        }
    }

    private void InteractWithHealthItem(HealthBoost healthBoost)
    {
        if (health <= maxHealth)
        {
            health += healthBoost.GetHealthBoost();
            healthBarUI.SetHealthBar(health, maxHealth);
        }
        healthBoost.OnHit();
    }
    void Die()
    {
        this.gameObject.SetActive(false);
        //play animation
        //game over display
        //stop game time
    }

    #endregion
}
