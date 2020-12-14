using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] SpawnOrigin;
    //public Rigidbody2D spawnBody;
    public GameObject spawnClone;

    public int spawnCount;
    public float spawnTimer = 3f;

    private float spawnTimerMax = 0f;
    private bool spawnAllowed = false;




    private void Start()
    {
        InvokeRepeating("SpawnNow", 1f, spawnTimer);
    }

    private void Update()
    {
        spawnTimerMax = Time.deltaTime;
        Debug.Log("timer info" + spawnTimerMax);
    }

    void SpawnNow()
    {


        if (spawnTimer >= spawnTimerMax)
        {
            /*calls spawn randomly at transforms */
            int indexRange = Random.Range(0, SpawnOrigin.Length);
            Instantiate(spawnClone, SpawnOrigin[0].position, Quaternion.identity);
            spawnCount++;
        }

        if (spawnCount == 5)
        {
            spawnAllowed = false;
            CancelInvoke("SpawnNow");
        }
    }

}
