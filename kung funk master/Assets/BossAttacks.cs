using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public Transform attackPoint, projectingPoint;
    [SerializeField] private int hitDamage = 100;
    [SerializeField] private float hitRadius = .50f;
    [SerializeField] private float DelayAttack = 1f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float timer;
    [SerializeField] private float maxTimer = 10f;


    [NonSerialized] public Rigidbody2D rb2D;
    public Animator anim;
    private AnimationClip clip;
    private AnimationEvent evt;

    private int bellyAttack = Animator.StringToHash("isAttacking");
    private int projectingAttack = Animator.StringToHash("isProjecting");
    public bool isBellyBashing = false;



    static BossAttacks _instance;
    public static BossAttacks instance
    {
        get
        {
            if (_instance == null)
            {

                _instance = FindObjectOfType<BossAttacks>();
            }

            return _instance;
        }
    }

    public void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        timer = maxTimer;
    }

    // Update is called once per frame
    void Update()
    {
        BossAttacking();
        CountDownToProjectile();
    }

    private void BossAttacking()
    {
        float attackBuffer = Vector2.Distance(transform.position, Player.instance.box2d.transform.position);

        if (attackBuffer <= 5)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            anim.SetBool(bellyAttack, true);
            Collider2D playerHit = Physics2D.OverlapCircle(attackPoint.transform.position, hitRadius, playerMask);

            if (playerHit != null)
            {

                playerHit.GetComponent<Player>().ReceiveDamage(hitDamage);
                isBellyBashing = true;
            }
            else
            {
                return;
            }
        }

        else
        {
            anim.SetBool(bellyAttack, false);
            isBellyBashing = false;
            rb2D.constraints = RigidbodyConstraints2D.None;
        }
    }

    private void ProjectFire()
    {
        float attackBuffer = Vector2.Distance(transform.position, Player.instance.box2d.transform.position);

        if (attackBuffer >= 6f)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            anim.SetTrigger(projectingAttack);
            //GameObject fire = PoolManager.instance.ReuseObject("Fired", projectingPoint.transform.position, Quaternion.identity);
            //fire.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, 0);
            //Debug.Log("Bang Fire");
        }
        else
        {
            //anim.SetBool(projectingAttack, false);
            rb2D.constraints = RigidbodyConstraints2D.None;
        }
    }


    private void CountDownToProjectile()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ProjectFire();
            ResetTimerBetweenProjectiles();
        }
    }
    private void ResetTimerBetweenProjectiles()
    {
        timer = maxTimer;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.transform.position, hitRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(projectingPoint.transform.position, .15f);

    }
}
