using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PManager : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] prefabOrigin;
    private int index;

    [Header("Spawn Timer")]
    public float timer = 3f;

    [Header("Spawn Dispatch Info")]
    public int allowEnemyCount = 3;
    [SerializeField] private int mixedEnemies;
    public float strongSpawnIntervals = 5f;
    public float spawnIntervals = 3f;
    public float waveIntervals;

    [Header("SFX")]
    [SerializeField] private AudioClip[] waveStartSFX;
    [SerializeField] [Range(0, 1)] private float sfxVolume = .7f;


    private void Start()
    {
        StartCoroutine(MixedSpawnWave());

    }

    private IEnumerator SpawnNow()
    {
        yield return new WaitForSeconds(waveIntervals);
        ChooseWaveIntroSFX();
        for (int i = 0; i < allowEnemyCount; i++)
        {
            int index = Random.Range(0, prefabOrigin.Length);
            PoolManager.instance.ReuseObject("weak", prefabOrigin[index].position, Quaternion.identity);
            yield return new WaitForSeconds(spawnIntervals);
        }

        yield return new WaitForSeconds(waveIntervals);
    }

    private IEnumerator SpawnAllWavesNow()
    {
        yield return StartCoroutine(SpawnNow());
        ChooseWaveIntroSFX();
        for (int i = 0; i < allowEnemyCount; i++)
        {
            int index = Random.Range(0, prefabOrigin.Length);
            PoolManager.instance.ReuseObject("strong", prefabOrigin[0].position, Quaternion.identity);
            yield return new WaitForSeconds(strongSpawnIntervals);
        }

        yield return new WaitForSeconds(waveIntervals);
    }

    IEnumerator MixedSpawnWave()
    {
        yield return StartCoroutine(SpawnAllWavesNow());
        ChooseWaveIntroSFX();
        for (int i = 0; i < mixedEnemies; i++)
        {
            int enemyRandom = Random.Range(0, 10);
            if (enemyRandom <= 5)
            {
                int index = Random.Range(0, prefabOrigin.Length);
                PoolManager.instance.ReuseObject("strong", prefabOrigin[0].position, Quaternion.identity);
                yield return new WaitForSeconds(strongSpawnIntervals);
            }
            if (enemyRandom >= 5)
            {
                int index = Random.Range(0, prefabOrigin.Length);
                PoolManager.instance.ReuseObject("weak", prefabOrigin[index].position, Quaternion.identity);
                yield return new WaitForSeconds(spawnIntervals);
            }
        }
    }

    private void ChooseWaveIntroSFX()
    {
        int sfxRandom = Random.Range(0, 2);
        AudioSource.PlayClipAtPoint(waveStartSFX[sfxRandom], Camera.main.transform.position, sfxVolume);
    }
}
