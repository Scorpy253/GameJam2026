using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 2f;
    private float timer;
    void Start()
    { 

    }
    void Update()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }
    void SpawnEnemy()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject obj = Instantiate(enemyPrefab, point.position, Quaternion.identity);
        obj.GetComponent<EnemyAI>().spdMulti = 1.5f;
        obj.GetComponent<EnemyAI>().dmgMulti = 1.5f;
        obj.GetComponent<EnemyAI>().hpMulti = 1f;
    }
}