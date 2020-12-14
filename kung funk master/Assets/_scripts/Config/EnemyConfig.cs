using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy Config" )]
public class EnemyConfig :ScriptableObject
{
    [SerializeField]
    public GameObject enemyPrefab;
    [SerializeField]
    public float moveSpeed;
    [SerializeField]
    public float spawnIntervaltimer = 1f;
    [SerializeField]
    public float randomSpawnIntervaltimer = .5f;

}
