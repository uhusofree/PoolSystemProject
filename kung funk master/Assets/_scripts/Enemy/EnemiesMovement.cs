using UnityEngine;

public class EnemiesMovement : MonoBehaviour, IPoolInterface
{
    public float mSpeed = 5;
    private Transform left, right;
    private Vector3 localScale;
    public bool movingR = true;

    public Rigidbody2D rb;

    static EnemiesMovement _instance;

    public static EnemiesMovement instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemiesMovement>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        right = GameObject.FindGameObjectWithTag("right").GetComponent<Transform>();
        left = GameObject.FindGameObjectWithTag("left").GetComponent<Transform>();
    }
    public void OnObjectSpawn()
    {
    
    }

    private void FixedUpdate()
    {
      
        if (transform.position.x > right.position.x)
        {
            movingR = false;
        }
        if (transform.position.x < left.position.x)
        {
            movingR = true;
        }

        if (movingR)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }

    }

    private void MoveRight()
    {

        movingR = true;
        localScale.x = 1;
        transform.localScale = localScale;
        transform.rotation = Quaternion.Euler(0, -180, 0);
        rb.velocity = new Vector2(localScale.x * mSpeed, rb.velocity.y); ;
    }

    private void MoveLeft()
    {
        movingR = false;
        localScale.x = -1;
        transform.localScale = localScale;
        transform.rotation = Quaternion.Euler(0, 180, 0);
        rb.velocity = new Vector2(localScale.x * mSpeed, rb.velocity.y);
    }

}

