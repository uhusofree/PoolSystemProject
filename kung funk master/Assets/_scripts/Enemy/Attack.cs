using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IPoolInterface
{
    [Header("Detection Info")]
    [SerializeField] private float rayLength = 4f;
    [SerializeField] private bool hasSeenTarget = false;
    [SerializeField] private Vector3 offsetRayCast;
    public LayerMask playerLayerMask;
    public Transform damagePoint;
    public int hitDamage;
    public float hitRadius = .22f;

    [Header("Attack Timer")]
    [SerializeField] private float delayAttacks = 3f;
    [SerializeField] private float attackTimer = 0f;
    [SerializeField] public bool canAttack = false;
    [SerializeField] private AudioClip bashSFX;
    [SerializeField] [Range(0, 1)] private float SFXvol; 

    Rigidbody2D rb2d;

    public Animator anim;

    static Attack _instance;
    public static Attack instance
    {
        get
        {
            if (_instance == null)
            {

                _instance = FindObjectOfType<Attack>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        
        rb2d = GetComponent<Rigidbody2D>();
        canAttack = false;
    }

    public void OnObjectSpawn()
    {
        attackTimer = delayAttacks;
        //hasSeenTarget = Physics2D.Raycast(transform.position, Vector3.left, rayLength);
    }

    void Update()
    {
        //Debug.Log(canAttack);

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else if (attackTimer <= 0)
        {
            Attacking();
            attackTimer = delayAttacks;
        }

    }

    #region WEAK ENEMY ATTACK

    private void Attacking()
    {
        //bool rayDirection = EnemiesMovement.instance.movingR;
        //Debug.Log("ray direction = " + rayDirection);

        //hasSeenTarget = Physics2D.Raycast(transform.position + offsetRayCast, !rayDirection ? Vector2.left : Vector2.right, rayLength, playerLayerMask);


        float attackBuffer = Vector2.Distance(transform.position, Player.instance.box2d.transform.position);


        if (attackBuffer <= 4)
        {
            Debug.Log("Buffer is " + attackBuffer);
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
            anim.SetBool("isAttacking", true);

            Collider2D playerHit = Physics2D.OverlapCircle(damagePoint.position, hitRadius, playerLayerMask);

            if (playerHit != null)
            {
                playerHit.GetComponent<Player>().ReceiveDamage(hitDamage);
                AudioSource.PlayClipAtPoint(bashSFX, Camera.main.transform.position, SFXvol);
            }
            else
            {
                return;
            }
        }

        else
        {
            anim.SetBool("isAttacking", false);
            rb2d.constraints = RigidbodyConstraints2D.None;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + offsetRayCast, transform.position + offsetRayCast + Vector3.left * rayLength);
        //Gizmos.DrawLine(transform.position - offsetRayCast, transform.position - offsetRayCast + Vector3.left * rayLength);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(damagePoint.position, hitRadius);

    }
}
