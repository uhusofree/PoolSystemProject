using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Adjustable Health Info")]
    public int currentHealth;
    [SerializeField] private int maxHealth = 100;

    [Header("VFX")]
    [SerializeField] public Animator anim;
    private int isHurting = Animator.StringToHash("takeHit");
    [SerializeField] private GameObject bloodSpurt;
    public Transform hitPoint;
    public bool isBeingHit = false;

    [Header("SFX")]
    [SerializeField] private AudioClip[] hitSFX;
    [SerializeField] [Range(0, 1)] private float SFXvolume = .7f;


    static Health _instance;
    public static Health instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Health>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int Damage)
    {
        AudioSource.PlayClipAtPoint(Player.instance.punch ? hitSFX[0] : hitSFX[1], Camera.main.transform.position, SFXvolume);
        isBeingHit = true;
        anim.SetTrigger(isHurting);
        currentHealth -= Damage;

        PoolManager.instance.ReuseObject("blood", hitPoint.position, hitPoint.rotation);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()

    {
        GameSession.instance.AddtoScore(maxHealth * 4);
        Attack.instance.canAttack = false;
        int chanceForHealth = Random.Range(0, 20);
        int chanceForInvulnerability = Random.Range(0, 100);
        int dropChance = Random.Range(0, 20);
        
        //Debug.Log("drop = " + dropChance);
        //Debug.Log("Health = " + chanceForHealth);
        //Debug.Log("invulnerable = " + chanceForInvulnerability);


        if (dropChance >= 10)
        {
            if (chanceForHealth <= 4)
            {
                PoolManager.instance.ReuseObject("HealthBoost", new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            }
        }
        if (dropChance <= 10)
        {
            if (chanceForInvulnerability <= 10)
            {
                PoolManager.instance.ReuseObject("InvulnerabilityBoost", new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            }
        }


        this.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(hitPoint.position, .3f);
    }
}
