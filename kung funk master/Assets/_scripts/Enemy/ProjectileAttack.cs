using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [Header("Projectile Attack")]
    [SerializeField] private float throwRayLength = 4f;
    [SerializeField] private Vector3 offsetRay;
    public Transform throwPoint;
    public Transform lowThrowPoint;

    [SerializeField] private float projectileTimer = 3;
    [SerializeField] private float minTimeBtwnAttacks = 0;
    [SerializeField] private float maxTimeBtwnAttacks = 0;
    [SerializeField] private float minChance = 0f;
    [SerializeField] private float maxChance = 10f;
    public bool canThrow = false;
    public LayerMask playerLayerMask;
    private float hitRadius = .22f;

    private Rigidbody2D rb2D;


    static ProjectileAttack _instance;

    public static ProjectileAttack instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ProjectileAttack>();
            }
            return _instance;
        }
    }

    public Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rb2D = EnemiesMovement.instance.rb;
    }

    // Update is called once per frame
    void Update()
    {
        CountDownToAttack();
    }

    private void ThrowProjectile()
    {
        bool rayDirection = EnemiesMovement.instance.movingR;
        canThrow = Physics2D.Raycast(transform.position + offsetRay, rayDirection ? Vector2.right : Vector2.left, throwRayLength, playerLayerMask);

        Debug.Log("can throw " + canThrow);
        //float attackBuffer = Vector2.Distance(transform.position, Player.instance.box2d.transform.position);
        float attackChance = Random.Range(minChance, maxChance);
        //Debug.Log("Attack chance value= " + attackChance);

        if (canThrow)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            //Attack.instance.canAttack = true;

            if (attackChance >= 5f)
            {
                HighThrow();
            }
            else
            {
                LowThrow();
            }

        }
        else
        {

            //Attack.instance.canAttack = false;
            anim.SetBool("isThrowing", false);
            anim.SetBool("isLowThrow", false);
            rb2D.constraints = RigidbodyConstraints2D.None;
        }

    }

    private void LowThrow()
    {
        anim.SetBool("isLowThrow", true);
        GameObject hatProjectile = PoolManager.instance.ReuseObject("hat", lowThrowPoint.position, Quaternion.identity);
    }

    private void HighThrow()
    {
        anim.SetBool("isThrowing", true);
        GameObject hatProjectile = PoolManager.instance.ReuseObject("hat", throwPoint.position, Quaternion.identity);
    }

    private void CountDownToAttack()
    {
        projectileTimer -= Time.deltaTime;

        if (projectileTimer <= 0)
        {
            ThrowProjectile();
            ResetAttackTimer();
        }
    }

    private void ResetAttackTimer()
    {
        projectileTimer = Random.Range(minTimeBtwnAttacks, maxTimeBtwnAttacks);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(throwPoint.position, hitRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lowThrowPoint.position, hitRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + offsetRay, transform.position + offsetRay + Vector3.left * throwRayLength);
    }
}
