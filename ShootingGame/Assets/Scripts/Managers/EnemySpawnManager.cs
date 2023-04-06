using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private Enemy[] spawnableEnemies;
    [SerializeField] private Enemy boss;

    [Header("SpawnRange")]
    [SerializeField] private float minSpawnX;
    [SerializeField] private float maxSpawnX;

    [Header("SpawnTime")]
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;

    [SerializeField] private float bossSpawnTime;

    private bool canSpawn;

    private void Start()
    {
        canSpawn = true;
        StartCoroutine(SpawnEnemyCoroutine());
        StartCoroutine(SpawnBossCoroutine());
    }

    private void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnableEnemies.Length);
        float spawnRangeX = Random.Range(minSpawnX, maxSpawnX);

        Enemy spawnedEnemy = spawnableEnemies[spawnIndex];

        Instantiate(spawnedEnemy, new Vector3(spawnRangeX, transform.position.y, transform.position.z), Quaternion.identity);
    }

    public IEnumerator SpawnEnemyCoroutine()
    {
        while (canSpawn)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }

    public void SpawnStageBoss()
    {
        Instantiate(boss, transform.position, Quaternion.identity);
    }

    private IEnumerator SpawnBossCoroutine()
    {
        yield return new WaitForSeconds(bossSpawnTime);
        canSpawn = false;
        SpawnStageBoss();
    }
}
