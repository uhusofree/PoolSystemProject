using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Transform test;
    [SerializeField] private float projectileSpeed = 20;
    BossAttacks BA;
    private void Start()
    {
        BA = GetComponent<BossAttacks>();
    }
    public void SpawnFireBall()
    {
        GameObject fire = PoolManager.instance.ReuseObject("Fired", test.transform.position, Quaternion.identity);
        fire.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, 0);
    }
}
